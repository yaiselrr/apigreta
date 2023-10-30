using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Entities.Dto;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.Api.Sqlserver;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.Sdk.ExternalScale.Enums;
using Greta.Sdk.ExternalScale.Model;
using Greta.Sdk.FileStorage.Interfaces;
using Greta.Sdk.FileStorage.Options;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Greta.BO.BusinessLogic.Handlers.Command.ExternalScale;

/// <summary>
/// Send data to one external scale  
/// </summary>
/// <param name="Store">Store id</param>
/// <param name="Department">Department Id</param>
/// <param name="Type">New Type</param>
/// <param name="Partial">Bool Value</param>
public record SendDataToExternalScaleCommand
(long Store, long Department, ExternalScaleOperationType Type,
    bool Partial) : IRequest<SendDataToExternalScaleResponse>;

/// <inheritdoc />
public record SendDataToExternalScaleResponse : CQRSResponse<List<ExternalScaleResponseHolder>>;

/// <inheritdoc />
public class
    SendDataToExternalScaleHandler : IRequestHandler<SendDataToExternalScaleCommand, SendDataToExternalScaleResponse>
{
    private readonly ILogger _logger;
    private readonly IConfiguration _configuration;
    private readonly IStorageProvider _storage;
    private readonly StorageOption _options;

    /// <summary>
    /// 
    /// </summary>
    public const string FolderTemplate = "Clients/{0}/externalScaleSyncFiles";

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="configuration"></param>
    /// <param name="storage"></param>
    /// <param name="options"></param>
    public SendDataToExternalScaleHandler(
        ILogger<SendDataToExternalScaleHandler> logger,
        IConfiguration configuration,
        IStorageProvider storage,
        IOptions<StorageOption> options
    )
    {
        _logger = logger;
        _configuration = configuration;
        _storage = storage;
        _options = options.Value;
    }

    /// <inheritdoc />
    public async Task<SendDataToExternalScaleResponse> Handle(SendDataToExternalScaleCommand request,
        CancellationToken cancellationToken)
    {
        var connectionString = _configuration.GetConnectionString("DefaultConnection");

        var options = new JsonSerializerOptions
        {
            WriteIndented = false
        };

        var b = new DbContextOptionsBuilder()
            .UseNpgsql(connectionString, sqlopt =>
            {
                sqlopt.UseAdminDatabase("defaultdb");
                sqlopt.EnableRetryOnFailure(10, TimeSpan.FromSeconds(10),
                    null);
            });

        await using (var context = new SqlServerContext(b.Options))
        {
            var externalScales = await context.Set<Api.Entities.ExternalScale>()
                .Include(x => x.Departments)
                .Where(x =>
                    x.State &&
                    x.StoreId == request.Store &&
                    x.ExternalScaleType != BoExternalScaleType.GretaLabel &&
                    x.Departments.Any(d => d.Id == request.Department)
                ).ToListAsync(cancellationToken: cancellationToken);

            var dtos = new Dictionary<Api.Entities.ExternalScale, SendScaleDataDto>();
            var result = new List<ExternalScaleResponseHolder>();
            foreach (var scale in externalScales)
            {
                var data = await PrepareFile(context, request, scale);
                if (data != null)
                {
                    dtos.Add(scale, data);
                }
            }

            foreach (var k in dtos.Keys)
            {
                var json = JsonSerializer.Serialize(dtos[k], options);
                var extJob = new ExternalJob()
                {
                    State = true,
                    Status = ExternalJobStatus.Init,
                    Type = ExternalJobType.ScaleUpdate,
                    Data = json,
                    RawData = $"{k.Id.ToString()}-{((int)request.Type).ToString()}",
                    UserCreatorId = "System"
                };
                await context.AddAsync(extJob, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);
                if (extJob.Id > 0)
                {
                    result.Add(new ExternalScaleResponseHolder() { Id = extJob.Id, Scale = k.Ip });
                }
            }

            return new SendDataToExternalScaleResponse() { Data = result };
        }
    }

    private async Task<SendScaleDataDto> PrepareFile(SqlServerContext context, SendDataToExternalScaleCommand request,
        Api.Entities.ExternalScale externalScale)
    {
        string path = null;
        switch (request.Type)
        {
            case ExternalScaleOperationType.Department:
                path = await GetDepartmentData(context, externalScale,
                    !request.Partial ? externalScale.LastDepartmentUpdate : null);
                break;
            case ExternalScaleOperationType.Category:
                path = await GetCategoryData(context, externalScale, request.Department,
                    !request.Partial ? externalScale.LastCategoryUpdate : null);
                break;
            case ExternalScaleOperationType.Product:
                path = await GetProductData(context, externalScale, request.Department,
                    !request.Partial ? externalScale.LastPluUpdate : null);
                break;
        }

        try
        {
            var enterprice = _options.RootFolder;
            var folder = string.Format(FolderTemplate, enterprice); // $"Clients/{enterprice}/externalScale";
            return new SendScaleDataDto()
            {
                FilePath = $"{folder}/{path}",
                Type = (int)request.Type,
                Action = 0,
                ScaleType = externalScale.ExternalScaleType,
                Ip = externalScale.Ip,
                Port = externalScale.Port
            };
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error when send data to scale");
            return null;
        }
    }

    #region Department

    private async Task<string> GetDepartmentData(SqlServerContext context, Api.Entities.ExternalScale es,
        DateTime? lastDate)
    {
        var guid = DateTime.Now.Ticks.ToString();
        var storeFilename = $"scale_department_{es.StoreId}_{guid}.gfb";

        var departments = await GetAllDepartmentForScales(context);

        if (departments.Count == 0)
        {
            throw new BussinessValidationException("No data to send");
        }

        return await UploadData(storeFilename, guid, new ScaleFileHolder() { Deps = departments });
    }

    private async Task<List<Greta.Sdk.ExternalScale.Model.DepartmentModel>> GetAllDepartmentForScales(
        SqlServerContext context)
    {
        return await context.Set<Api.Entities.Department>()
            .Where(x => x.Perishable && x.State)
            .Select(x => new Greta.Sdk.ExternalScale.Model.DepartmentModel()
            {
                Id = x.DepartmentId,
                Name = x.Name
            })
            .ToListAsync();
    }

    #endregion

    #region Category

    private async Task<string> GetCategoryData(SqlServerContext context, Api.Entities.ExternalScale es,
        long departmentId,
        DateTime? lastDate)
    {
        var guid = DateTime.Now.Ticks.ToString();
        var storeFilename = $"scale_category_{es.StoreId}_{guid}.gfb";

        var categories = (lastDate.HasValue)
            ? await GetUpdatesCategoryForScales(context, lastDate.Value,
                departmentId)
            : await GetAllCategoryForScales(context, departmentId);

        if (categories.Count == 0)
        {
            throw new BussinessValidationException("No data to send");
        }

        return await UploadData(storeFilename, guid, new ScaleFileHolder() { Cats = categories });
    }

    private async Task<List<Greta.Sdk.ExternalScale.Model.CategoryModel>> GetUpdatesCategoryForScales(
        SqlServerContext context, DateTime last, long dep)
    {
        return await context.Set<Api.Entities.ScaleCategory>()
            .Include(x => x.Parent)
            .Where(x =>
                x.State &&
                (
                    x.CreatedAt > last ||
                    x.UpdatedAt > last
                ) && dep == x.DepartmentId)
            .Select(x => new Greta.Sdk.ExternalScale.Model.CategoryModel()
            {
                Id = x.CategoryId,
                Name = x.Name,
                DepartmentId = x.DepartmentId,
                ParentID = x.Parent != null ? x.Parent.CategoryId : 0
            })
            .ToListAsync();
    }

    private async Task<List<Greta.Sdk.ExternalScale.Model.CategoryModel>> GetAllCategoryForScales(
        SqlServerContext context, long dep)
    {
        return await context.Set<Api.Entities.ScaleCategory>()
            .Include(x => x.Parent)
            .Where(x =>
                x.State && dep == x.DepartmentId)
            .Select(x => new Greta.Sdk.ExternalScale.Model.CategoryModel()
            {
                Id = x.CategoryId,
                Name = x.Name,
                DepartmentId = x.DepartmentId,
                ParentID = x.Parent != null ? x.Parent.CategoryId : 0
            })
            .ToListAsync();
    }

    #endregion

    private async Task<string> GetProductData(SqlServerContext context, Api.Entities.ExternalScale es, long departmentId,
        DateTime? lastDate)
    {
        var guid = DateTime.Now.Ticks.ToString();
        var storeFilename = $"scale_product_{es.StoreId}_{guid}.gfb";

        var products = (lastDate.HasValue)
            ? await GetUpdatesForScales(context, es.StoreId, lastDate.Value,
                departmentId)
            : await GetAllForScales(context, es.StoreId, departmentId);
        if (products.Count == 0)
        {
            throw new BussinessValidationException("No data to send");
        }

        return await UploadData(storeFilename, guid, new ScaleFileHolder() { Products = products });
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="storeId"></param>
    /// <param name="last"></param>
    /// <param name="dep"></param>
    public async Task<List<PLUModel>> GetUpdatesForScales(SqlServerContext context,
        long storeId, DateTime last, long dep)
    {
        var products = await context.Set<ScaleProduct>()
            .Include(x => x.StoreProducts)
            .Include(x => x.Department)
            .Include(x => x.ScaleCategory)
            .Include(x => x.ScaleLabelDefinitions).ThenInclude(s => s.ScaleLabelType1)
            .Where(x =>
                x.State &&
                (
                    x.CreatedAt > last ||
                    x.UpdatedAt > last
                ) && dep == x.DepartmentId)
            .ToListAsync();
        return ToModel(products, storeId);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="storeId"></param>
    /// <param name="dep"></param>
    public async Task<List<PLUModel>> GetAllForScales(SqlServerContext context,
        long storeId, long dep)
    {
        var products = await context.Set<ScaleProduct>()
            .Include(x => x.StoreProducts)
            .Include(x => x.Department)
            .Include(x => x.ScaleCategory)
            .Include(x => x.ScaleLabelDefinitions).ThenInclude(s => s.ScaleLabelType1)
            .Where(x =>
                x.State && dep == x.DepartmentId)
            .ToListAsync();

        return ToModel(products, storeId);
    }

    private List<PLUModel> ToModel(List<ScaleProduct> products, long storeId)
    {
        var result = new List<PLUModel>();
        foreach (var x in products)
        {
            var st = x.StoreProducts.Find(x => x.StoreId == storeId);
            if (st != null)
            {
                long pluId = x.PLUNumber;
                if (x.UPC.Length == 5 && long.TryParse(x.UPC, out var pluIdTemp))
                {
                    pluId = pluIdTemp;
                }
                
                result.Add(new PLUModel()
                {
                    Id = pluId,
                    ItemCode = x.PLUNumber.ToString(),
                    Name1 = x.Name,
                    Name2 = null,
                    Name3 = null,
                    DepartmentId = x.Department.DepartmentId,
                    GroupId = x.ScaleCategory.CategoryId,
                    Price = st.Price,
                    UnitId = x.PLUType == PluType.RandomWeight ? UnitId.LB : UnitId.PCS,
                    Text1 = x.Text1,
                    Text2 = x.Text2,
                    Text3 = x.Text3,
                    Label1ID = x.ScaleLabelDefinitions.Count() == 0
                        ? 0
                        : x.ScaleLabelDefinitions[0].ScaleLabelType1.LabelId,
                    FreshnessDate = x.ShelfLife,
                    ValidDate = x.ProductLife,
                    TareValue = st.Product.Tare1
                });
            }
        }

        return result;
    }

    private async Task<string> UploadData(string storeFilename, string guid, object datas)
    {
        var enterprice = _options.RootFolder;
        var folder = string.Format(FolderTemplate, enterprice); // $"Clients/{enterprice}/externalScale";
        var fileName = Path.GetTempPath() + guid + ".dat";
        var data = SerializeObject(datas);
        await File.WriteAllBytesAsync(fileName, GetZipArchive("file.dat", data));
        return await _storage.Upload(folder, fileName,
            useSameFilename: storeFilename);
    }

    private byte[] GetZipArchive(string filename, byte[] content)
    {
        byte[] archiveFile;
        using var archiveStream = new MemoryStream();
        using (var archive = new ZipArchive(archiveStream, ZipArchiveMode.Create, true))
        {
            var zipArchiveEntry = archive.CreateEntry(filename, CompressionLevel.Fastest);
            using (var zipStream = zipArchiveEntry.Open())
            {
                zipStream.Write(content, 0, content.Length);
            }
        }

        archiveFile = archiveStream.ToArray();

        return archiveFile;
    }

    private byte[] SerializeObject(object data)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = false
        };
        return JsonSerializer.SerializeToUtf8Bytes(data, options);
    }
}


/// <summary>
/// 
/// </summary>
public class ExternalScaleResponseHolder
{
    /// <summary>
    /// 
    /// </summary>
    public long Id { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string Scale { get; set; }
}