using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Greta.BO.BusinessLogic.Hubs;
using Greta.BO.BusinessLogic.Models.Hubs;
using Greta.BO.BusinessLogic.Service;
using Greta.Sdk.FileStorage.Interfaces;
using Greta.Sdk.FileStorage.Options;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Greta.BO.BusinessLogic.Handlers.Command.Synchro
{
    public record SynchroFullBackupCommand(Guid StoreId, string ConnectionId, bool FromWorker = true) : IRequest;

    public class SynchroFullBackupHandler : IRequestHandler<SynchroFullBackupCommand>
    {
        private readonly IBoHubClient _hub;
        private readonly ILogger _logger;
        private readonly IHubContext<CloudHub, ICloudHub> _hubCloud;
        private readonly ISynchroService _service;
        private readonly StorageOption _sOptions;
        private readonly IStorageProvider _storage;
        private readonly IStoreService _storeService;
        private readonly IDeviceService _deviceService;

        public SynchroFullBackupHandler(
            IStoreService storeService,
            IDeviceService deviceService,
            IBoHubClient hub,
            ISynchroService service,
            ILogger<SynchroFullBackupHandler> logger,
            IOptions<StorageOption> options,
            IStorageProvider storage,
            // IHubContext<CloudHub, ICloudHub> hubCloud,
            IServiceProvider provider
        )
        {
            _storeService = storeService;
            _deviceService = deviceService;
            _logger = logger;

            // _hubCloud = hubCloud;
            _service = service;
            _hub = hub;
            _storage = storage;
            _sOptions = options.Value;
            try
            {
                _hubCloud = provider.GetRequiredService<IHubContext<CloudHub, ICloudHub>>();
            }
            catch
            {
                // ignored
            }
        }

        private async Task NotifyStatus(bool fromWorker, int status, string connectionId, string message,
            object data)
        {
            try
            {
                if (fromWorker)
                {
                    await _hub.OnNotifyWorkerFullBackupStatus(status,
                        connectionId,
                        message,
                        data);
                }
                else
                {
                    if (_hubCloud == null) return;
                    var toSend = new NotifyWorkerFullBackupStatus
                    {
                        StatusCode = status,
                        Errors = new List<string> { message },
                        Data = data,
                        ConnectionId = connectionId
                    };

                    await _hubCloud.Clients.Client(connectionId).OnCompleteFullBackup(toSend);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Worker hub notification error");
            }
        }

        public async Task Handle(SynchroFullBackupCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Getting store information");
                var store = await _storeService.GetByGuid(request.StoreId);
                if (store == null)
                {
                    _logger.LogError("Failed to create full backup {StoreId} store not found", request.StoreId);
                    if (request.ConnectionId != null)
                        await NotifyStatus(request.FromWorker, (int)HttpStatusCode.BadRequest,
                            request.ConnectionId,
                            $"Synchronization data  for id {request.StoreId} store not found.", null);
                    return;
                }

                var enterprice = _sOptions.RootFolder;
                var folder = $"Clients/{enterprice}/fullbackupfolder";
                var storeFilename = $"full_backup_{store.Id}_{store.LastBackupVersion}.gfb";

                //if (store.LastBackupPath != null && !store.LastBackupPath.Equals(""))
                //    if (store.SynchroVersion - 2 <= store.LastBackupVersion &&
                //        store.LastBackupTime > DateTime.Now.AddDays(-1))
                //    {
                //        _logger.LogInformation($"Using five days cache full backup in {store.LastBackupPath}");
                //        _logger.LogInformation(
                //            "Sending notification to the devices involved. (Not Implemented Yet)");
                //        if (request.connectionId != null)
                //            await NotifyStatus(request.fromWorker, (int) HttpStatusCode.OK,
                //                request.connectionId,
                //                $"Synchronization data  for store id {request.StoreId} is success.",
                //                store.LastBackupPath);
                //        return Unit.Value;
                //    }

                //notify for is preparing
                if (request.ConnectionId != null)
                    await NotifyStatus(request.FromWorker, (int)HttpStatusCode.OK,
                        request.ConnectionId, $"Synchronization data for store id {request.StoreId} is preparing.",
                        null);

                _logger.LogInformation("Getting the data from the database");

                var data = await _service.CreateAFullBackupFile(store.Id);

                _logger.LogInformation("Data from the database successfully obtained");

                var fileName = Path.GetTempPath() + Guid.NewGuid() + ".dat";
                _logger.LogInformation("Creating full backup file");

                var options = new JsonSerializerOptions
                {
                    WriteIndented = false
                };
                var jsonUtf8Bytes = JsonSerializer.SerializeToUtf8Bytes(data, options);

                await File.WriteAllBytesAsync(fileName, GetZipArchive("backup.bak", jsonUtf8Bytes), cancellationToken);

                _logger.LogInformation("Local file successfully created on the path {Filename}", fileName);

                _logger.LogInformation("Uploading file in to S3");


                var path = await _storage.Upload(folder, fileName,
                    useSameFilename: storeFilename);
                if (path == null) return;
                store.LastBackupPath = $"{folder}/{path}";
                store.LastBackupTime = DateTime.UtcNow;
                store.LastBackupVersion = store.SynchroVersion;

                //comment for testing
                await _storeService.Put(store.Id, store);

                _logger.LogInformation("Updating the store {StoreId}", request.StoreId);

                if (request.ConnectionId != null)
                {
                    await NotifyStatus(request.FromWorker, (int)HttpStatusCode.OK,
                        request.ConnectionId, $"Synchronization data  for id {request.StoreId} success.",
                        store.LastBackupPath);
                    //update device with this store.LastBackupVersion

                    var dev = await _deviceService.GetDeviceByConnectionId(request.ConnectionId);
                    dev.SynchroVersion = store.LastBackupVersion;
                    await _deviceService.Put(dev.Id, dev);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to create full backup");
                await NotifyStatus(request.FromWorker, (int)HttpStatusCode.BadRequest,
                    request.ConnectionId, $"Synchronization data  for id {request.StoreId} fail.", null);
            }

            return;
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
    }
}