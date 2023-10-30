using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Entities.Dto;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.Api.Sqlserver;
using Greta.BO.BusinessLogic.Handlers.Command.ExternalScale;
using Greta.Sdk.ExternalScale.Enums;
using Greta.Sdk.ExternalScale.Model;
using Greta.Sdk.FileStorage.Interfaces;
using Greta.Sdk.FileStorage.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Greta.BO.Api.Workers
{
    [ExcludeFromCodeCoverage]
    public class PriceBatchAdjustmentWorker : BaseWorker
    {
        private readonly ILogger<PriceBatchAdjustmentWorker> _logger;

        private readonly IConfiguration _configuration;

        // private readonly IHostApplicationLifetime _applicationLifetime;
        private readonly IStorageProvider _storage;
        private readonly IOptions<StorageOption> _options;

        public PriceBatchAdjustmentWorker(
            ILogger<PriceBatchAdjustmentWorker> logger,
            IConfiguration configuration,
            // IHostApplicationLifetime applicationLifetime,
            IOptions<StorageOption> options,
            IStorageProvider storage
        )
        {
            this._logger = logger;
            _storage = storage;
            this._configuration = configuration;
            // _applicationLifetime = applicationLifetime;
            _options = options;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = false
                };
                var connectionString = _configuration.GetConnectionString("DefaultConnection");
                while (!stoppingToken.IsCancellationRequested)
                {
                    var needSendScale = false;
                    try
                    {
                        var b = new DbContextOptionsBuilder()
                            .UseNpgsql(connectionString, sqlopt =>
                            {
                                sqlopt.UseAdminDatabase("defaultdb");
                                sqlopt.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                                sqlopt.EnableRetryOnFailure(10, TimeSpan.FromSeconds(10),
                                    null);
                            });

                        await using (var context = new SqlServerContext(b.Options))
                        {
                            var currentTime = DateTime.UtcNow;
                            //currentTime = DateTime.SpecifyKind(currentTime, DateTimeKind.Utc);


                            var count = await context.Set<PriceBatch>()
                                .Where(x => x.StartTime < currentTime && x.State)
                                .Select(x => x.Id)
                                .CountAsync(cancellationToken: stoppingToken);
                            if (count == 0) continue;

                            var fountPriceBatch = await ProcessPriceBatch(stoppingToken, context, currentTime);
                            if (fountPriceBatch) needSendScale = true;

                            var fountAdBatchStart = await ProcessAdBeginBatch(stoppingToken, context, currentTime);
                            if (fountAdBatchStart) needSendScale = true;

                            var fountAdBatchEnd = await ProcessAdEndBatch(stoppingToken, context, currentTime);
                            if (fountAdBatchEnd) needSendScale = true;


                            //process Add batch
                            var d = await context.SaveChangesAsync(stoppingToken) > 0;
                            try
                            {
                                //send data to devices if any 
                                if (needSendScale)
                                {
                                    var externalScales = await context.Set<ExternalScale>()
                                        .Include(x => x.Departments)
                                        .Where(x =>
                                            x.State &&
                                            x.ExternalScaleType != BoExternalScaleType.GretaLabel
                                        ).ToListAsync(cancellationToken: stoppingToken);
                                    var dtos = new Dictionary<ExternalScale, SendScaleDataDto>();
                                    foreach (var scale in externalScales)
                                    {
                                        var path = await GetProductData(context, scale);
                                        if (path != null)
                                        {
                                            var enterprise = _options.Value.RootFolder;
                                            var folder = string.Format(SendDataToExternalScaleHandler.FolderTemplate,
                                                enterprise);
                                            dtos.Add(scale, new SendScaleDataDto()
                                            {
                                                FilePath = $"{folder}/{path}",
                                                Type = (int)ExternalScaleOperationType.Product,
                                                Action = 0,
                                                ScaleType = scale.ExternalScaleType,
                                                Ip = scale.Ip,
                                                Port = scale.Port
                                            });
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
                                            RawData =
                                                $"{k.Id.ToString()}-{((int)ExternalScaleOperationType.Product).ToString()}",
                                            UserCreatorId = "System"
                                        };
                                        await context.AddAsync(extJob, stoppingToken);
                                        await context.SaveChangesAsync(stoppingToken);
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                _logger.LogError(e, "Problem sending information to scales");
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e, "Problem updating price batch");
                        //_applicationLifetime.StopApplication();
                    }
                    finally
                    {
                        await Task.Delay((int)TimeSpan.FromMinutes(1).TotalMilliseconds, stoppingToken);
                    }
                }

                await WaitForCancellationAsync(stoppingToken);
            }
            catch (Exception)
            {
                // _applicationLifetime.StopApplication();
            }
            finally
            {
                if (!stoppingToken.IsCancellationRequested)
                    _logger.LogError("Worker stopped unexpectedly");
            }

            _logger.LogInformation("Closing PriceBatchAdjustmentWorker");

            _logger.LogInformation("Dispose PriceBatchAdjustmentWorker");
        }

        private async Task<bool> ProcessAdEndBatch(CancellationToken stoppingToken, SqlServerContext context,
            DateTime currentTime)
        {
            bool needSendScale = false;
            var pbs = await context.Set<AdBatch>()
                .Include(x => x.PriceBatchDetails)
                .Include(x => x.Stores)
                .Where(x => x.Active && x.EndTime > currentTime && x.State)
                .ToListAsync(cancellationToken: stoppingToken);

            foreach (var pb in pbs)
            {
                var storesIds = pb.Stores.Select(x => x.Id).ToList();
                //review
                foreach (var det in pb.PriceBatchDetails)
                {
                    try
                    {
                        //adjust all products
                        if (det.ProductId != null)
                        {
                            //Process this product only
                            await ProcessProductsFromDetail(context, det.ProductId.Value, storesIds, null,
                                stoppingToken);
                        }

                        if (det.FamilyId != null)
                        {
                            //Process this family
                            await ProcessFamilyFromDetails(context, det.FamilyId.Value, storesIds, null, stoppingToken);
                        }

                        if (det.CategoryId != null)
                        {
                            //Process this category
                            await ProcessCategoryByDetail(context, det.CategoryId.Value, storesIds, null,
                                stoppingToken);
                        }

                        await context.SaveChangesAsync(stoppingToken);
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e, "Problem updating price batch detail");
                    }
                }

                //remove this pb of database if we are on device

                #region Remove pb

                pb.Active = true;
                context.Set<AdBatch>().Remove(pb);
                await context.SaveChangesAsync(stoppingToken);
                needSendScale = true;

                #endregion

                await Task.Delay((int)TimeSpan.FromSeconds(10).TotalMilliseconds, stoppingToken);
            }

            return needSendScale;
        }

        private async Task<bool> ProcessAdBeginBatch(CancellationToken stoppingToken, SqlServerContext context,
            DateTime currentTime)
        {
            bool needSendScale = false;
            var pbs = await context.Set<AdBatch>()
                .Include(x => x.PriceBatchDetails)
                .Include(x => x.Stores)
                .Where(x => !x.Active && x.StartTime < currentTime && x.State)
                .ToListAsync(cancellationToken: stoppingToken);

            foreach (var pb in pbs)
            {
                var storesIds = pb.Stores.Select(x => x.Id).ToList();
                //review
                foreach (var det in pb.PriceBatchDetails)
                {
                    try
                    {
                        //adjust all products
                        if (det.ProductId != null)
                        {
                            //Process this product only
                            await ProcessProductsFromDetail(context, det.ProductId.Value, storesIds, det.Price,
                                stoppingToken);
                        }

                        if (det.FamilyId != null)
                        {
                            //Process this family
                            await ProcessFamilyFromDetails(context, det.FamilyId.Value, storesIds, det.Price,
                                stoppingToken);
                        }

                        if (det.CategoryId != null)
                        {
                            //Process this category
                            await ProcessCategoryByDetail(context, det.CategoryId.Value, storesIds, det.Price,
                                stoppingToken);
                        }

                        await context.SaveChangesAsync(stoppingToken);
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e, "Problem updating price batch detail");
                    }
                }

                //remove this pb of database if we are on device

                #region Remove pb

                pb.Active = true;
                context.Set<AdBatch>().Update(pb);
                needSendScale = true;

                #endregion

                await Task.Delay((int)TimeSpan.FromSeconds(10).TotalMilliseconds, stoppingToken);
            }

            return needSendScale;
        }

        private async Task<bool> ProcessPriceBatch(CancellationToken stoppingToken, SqlServerContext context,
            DateTime currentTime)
        {
            bool needSendScale = false;
            var pbs = await context.Set<PriceBatch>()
                .Include(x => x.PriceBatchDetails)
                .Include(x => x.Stores)
                .Where(x => x.StartTime < currentTime && x.State)
                .ToListAsync(cancellationToken: stoppingToken);

            foreach (var pb in pbs)
            {
                var storesIds = pb.Stores.Select(x => x.Id).ToList();
                //review
                foreach (var det in pb.PriceBatchDetails)
                {
                    try
                    {
                        //adjust all products
                        if (det.ProductId != null)
                        {
                            //Process this product only
                            await ProcessProductsFromDetail(context, det.ProductId.Value, storesIds, det.Price,
                                stoppingToken);
                        }

                        if (det.FamilyId != null)
                        {
                            //Process this family
                            await ProcessFamilyFromDetails(context, det.FamilyId.Value, storesIds, det.Price,
                                stoppingToken);
                        }

                        if (det.CategoryId != null)
                        {
                            //Process this category
                            await ProcessCategoryByDetail(context, det.CategoryId.Value, storesIds, det.Price,
                                stoppingToken);
                        }

                        await context.SaveChangesAsync(stoppingToken);
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e, "Problem updating price batch detail");
                    }
                }

                //remove this pb of database if we are on device

                #region Remove pb

                context.Set<PriceBatch>().Remove(pb);
                needSendScale = true;

                #endregion

                await Task.Delay((int)TimeSpan.FromSeconds(10).TotalMilliseconds, stoppingToken);
            }

            return needSendScale;
        }

        private static async Task ProcessCategoryByDetail(SqlServerContext context,
            long categoryId, IReadOnlyCollection<long> stores, decimal? price, CancellationToken stoppingToken)
        {
            var tmpCategory = await context.Set<Category>()
                .Include(x => x.Products)
                .Where(x => x.Id == categoryId)
                .FirstOrDefaultAsync(cancellationToken: stoppingToken);

            var tmpProducts = await context.Set<StoreProduct>()
                .Where(x =>
                    tmpCategory.Products.Any(s => s.Id == x.ProductId) &&
                    stores.Contains(x.StoreId)
                )
                .ToListAsync(cancellationToken: stoppingToken);

            foreach (var tP in tmpProducts)
            {
                var tempPrice = tP.Price;
                if (price.HasValue)
                    tP.Price = price.Value;
                else
                {
                    if (tP.BatchOldPrice.HasValue)
                        tP.Price = tP.BatchOldPrice.Value;
                }

                tP.BatchOldPrice = tempPrice;
                context.Set<StoreProduct>().Update(tP);
            }
        }

        private static async Task ProcessFamilyFromDetails(SqlServerContext context,
            long familyId, IReadOnlyCollection<long> stores, decimal? price, CancellationToken stoppingToken)
        {
            var tmpFamily = await context.Set<Family>()
                .Include(x => x.Products)
                .Where(x => x.Id == familyId)
                .FirstOrDefaultAsync(cancellationToken: stoppingToken);

            var tmpProducts = await context.Set<StoreProduct>()
                .Where(x =>
                    tmpFamily.Products.Any(s => s.Id == x.ProductId) &&
                    stores.Contains(x.StoreId)
                )
                .ToListAsync(cancellationToken: stoppingToken);

            foreach (var tP in tmpProducts)
            {
                var tempPrice = tP.Price;
                if (price.HasValue)
                    tP.Price = price.Value;
                else
                {
                    if (tP.BatchOldPrice.HasValue)
                        tP.Price = tP.BatchOldPrice.Value;
                }

                tP.BatchOldPrice = tempPrice;
                context.Set<StoreProduct>().Update(tP);
            }
        }

        private static async Task ProcessProductsFromDetail(SqlServerContext context,
            long productId, IReadOnlyCollection<long> stores, decimal? price, CancellationToken stoppingToken)
        {
            var tmpProducts = await context.Set<StoreProduct>()
                .Where(x => x.ProductId == productId &&
                            stores.Contains(x.StoreId))
                .ToListAsync(cancellationToken: stoppingToken);
            foreach (var tP in tmpProducts)
            {
                var tempPrice = tP.Price;
                if (price.HasValue)
                    tP.Price = price.Value;
                else
                {
                    if (tP.BatchOldPrice.HasValue)
                        tP.Price = tP.BatchOldPrice.Value;
                }

                tP.BatchOldPrice = tempPrice;
                context.Set<StoreProduct>().Update(tP);
            }
        }

        private async Task<string> GetProductData(SqlServerContext context, ExternalScale es)
        {
            var guid = DateTime.Now.Ticks.ToString();
            var storeFilename = $"scale_product_{es.StoreId}_{guid}.gfb";

            var products = await GetAllForScales(context, es.StoreId, es.Departments?.Select(x => x.Id).ToList());
            if (products.Count == 0)
            {
                return null;
            }

            return await UploadData(storeFilename, guid, new ScaleFileHolder() { Products = products });
        }

        private async Task<string> UploadData(string storeFilename, string guid, object datas)
        {
            var enterprise = _options.Value.RootFolder;
            var folder = string.Format(SendDataToExternalScaleHandler.FolderTemplate, enterprise);
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

        public async Task<List<PLUModel>> GetUpdatesForScales(SqlServerContext context,
            long storeId, DateTime last, List<long> deps)
        {
            if (deps == null || deps.Count == 0)
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
                        )
                    )
                    .ToListAsync();
                return ToModel(products, storeId);
            }
            else
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
                        ) && deps.Any(d => d == x.DepartmentId))
                    .ToListAsync();
                return ToModel(products, storeId);
            }
        }

        private List<PLUModel> ToModel(List<ScaleProduct> products, long storeId)
        {
            var result = new List<PLUModel>();
            foreach (var x in products)
            {
                var st = x.StoreProducts.Find(x => x.StoreId == storeId);
                if (st != null)
                {
                    result.Add(new PLUModel()
                    {
                        Id = x.PLUNumber,
                        ItemCode = x.UPC,
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
                    });
                }
            }

            return result;
        }

        private async Task<List<PLUModel>> GetAllForScales(SqlServerContext context,
            long storeId, List<long> deps)
        {
            var products = await context.Set<ScaleProduct>()
                .Include(x => x.StoreProducts)
                .Include(x => x.Department)
                .Include(x => x.ScaleCategory)
                .Include(x => x.ScaleLabelDefinitions).ThenInclude(s => s.ScaleLabelType1)
                .Where(x =>
                    x.State && deps.Any(d => d == x.DepartmentId))
                //.Take(1)
                .ToListAsync();

            return ToModel(products, storeId);
        }
    }
}