using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Greta.BO.Api.Entities.Dto;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using Greta.Sdk.ExternalScale.Enums;
using Greta.Sdk.ExternalScale.Model;
using Greta.Sdk.FileStorage.Interfaces;
using Greta.Sdk.FileStorage.Options;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Greta.BO.BusinessLogic.Handlers.Command.OnlineStore;

/// <summary>
/// Send data to one online store 
/// </summary>
/// <param name="OnlineStore"></param>
/// <param name="Type"></param>
/// <param name="Partial"></param>
public record SendDataToOnlineStoreCommand(long OnlineStore, ExternalScaleOperationType Type, bool Partial) : IRequest<Response>
{
}

/// <inheritdoc />
public class SendDataToOnlineStoreHandler : IRequestHandler<SendDataToOnlineStoreCommand, Response>
{
    private readonly ILogger<SendDataToOnlineStoreHandler> _logger;
    private readonly IExternalJobService _externalJobService;
    private readonly IOnlineStoreService _onlineStoreService;
    private readonly IDepartmentService _departmentService;
    private readonly IStorageProvider _storage;
    private readonly StorageOption _options;

    /// <inheritdoc />
    public SendDataToOnlineStoreHandler(
        ILogger<SendDataToOnlineStoreHandler> logger,
        IExternalJobService externalJobService,
        IOnlineStoreService onlineStoreService,
        IDepartmentService departmentService,
        IStorageProvider storage,
        IOptions<StorageOption> options
    )
    {
        _logger = logger;
        _externalJobService = externalJobService;
        _onlineStoreService = onlineStoreService;
        _departmentService = departmentService;
        _storage = storage;
        _options = options.Value;
    }

    /// <inheritdoc />
    public async Task<Response> Handle(SendDataToOnlineStoreCommand request, CancellationToken cancellationToken)
    {

        var onlineStore = await _onlineStoreService.Get(request.OnlineStore);
        string path = null;
        switch (request.Type)
        {
            case ExternalScaleOperationType.Department:
                path = await GetDepartmentData(onlineStore, !request.Partial ? DateTime.Now : null);
                break;
        }
        try
        {
            var enterprice = _options.RootFolder;
            var folder = $"{enterprice}/online-store";
            var response = await _externalJobService.CreateTask(ExternalJobType.ScaleUpdate, new SendOnlineStoreDataDto()
            {
                FilePath = $"{folder}/{path}",
                Type = (int)request.Type,
                Action = 0,
                LocationServerType = onlineStore.LocationServerType,
                Name = onlineStore.Name,
                NameWebsite = onlineStore.NameWebsite
            }, $"{onlineStore.Id.ToString()}-{((int)request.Type).ToString()}");

            return new Response() { Data = response };
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error when send data to scale.");
            return new Response() { Data = -1 };
        }
    }

    /// <inheritdoc />
    public async Task<string> GetDepartmentData(Api.Entities.OnlineStore es, DateTime? lastDate)
    {
        var guid = DateTime.Now.Ticks.ToString();
        var storeFilename = $"online_store_department_{es.StoreId}_{guid}.gfb";

        var departments = (lastDate.HasValue) ?
            await _departmentService.GetUpdatesForScales(lastDate.Value, es.Departments?.Select(x => x.Id).ToList()) :
            await _departmentService.GetAllForScales(es.Departments?.Select(x => x.Id).ToList());

        if (departments.Count == 0)
        {
            throw new BussinessValidationException("No data to send");
        }

        return await UploadData(storeFilename, guid, new ScaleFileHolder() { Deps = departments });
    }

    private async Task<string> UploadData(string storeFilename, string guid, object datas)
    {
        var enterprice = _options.RootFolder;
        var folder = $"{enterprice}/scale";
        var fileName = Path.GetTempPath() + guid + ".dat";
        var data = SerializeObject(datas);
        File.WriteAllBytes(fileName, GetZipArchive("file.dat", data));
        return await _storage.Upload(folder, fileName,
            useSameFilename: storeFilename);
    }

    private byte[] GetZipArchive(string filename, byte[] content)
    {
        byte[] archiveFile;
        using (var archiveStream = new MemoryStream())
        {
            using (var archive = new ZipArchive(archiveStream, ZipArchiveMode.Create, true))
            {
                var zipArchiveEntry = archive.CreateEntry(filename, CompressionLevel.Fastest);
                using (var zipStream = zipArchiveEntry.Open())
                {
                    zipStream.Write(content, 0, content.Length);
                }
            }

            archiveFile = archiveStream.ToArray();
        }

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

/// <inheritdoc />
public record Response : CQRSResponse<long>;