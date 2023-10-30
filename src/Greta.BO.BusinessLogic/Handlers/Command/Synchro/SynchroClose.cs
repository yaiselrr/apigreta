using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Greta.BO.Api.Entities.Entensions;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.Api.Entities.Helpers;
using Greta.BO.Api.Entities.Lite;
using Greta.BO.BusinessLogic.Hubs;
using Greta.BO.BusinessLogic.Service;
using Greta.Sdk.FileStorage.Interfaces;
using Greta.Sdk.FileStorage.Options;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Greta.BO.BusinessLogic.Handlers.Command.Synchro
{
    public record SynchroCloseCommand(long Id) : IRequest;

    public class SynchroCloseValidator : AbstractValidator<SynchroCloseCommand>
    {
        private readonly ISynchroService _service;

        public SynchroCloseValidator(ISynchroService service)
        {
            _service = service;
            RuleFor(x => x.Id)
                .MustAsync(WithOutClosedSynchros).WithMessage("There can only be one synchronization in progress.");
        }

        private async Task<bool> WithOutClosedSynchros(long id, CancellationToken cancellationToken)
        {
            return await _service.HasSynchroInProgress(id);
        }
    }

    public class SynchroCloseHandler : IRequestHandler<SynchroCloseCommand>
    {
        private readonly IBoHubClient _hub;
        private readonly ILogger _logger;
        private readonly ISynchroService _service;
        private readonly IStoreService _storeService;
        private readonly StorageOption _sOptions;
        private readonly IStorageProvider _storage;

        public SynchroCloseHandler(
            IBoHubClient hub,
            ILogger<SynchroCloseHandler> logger,
            ISynchroService service,
            IStoreService storeService,
            IStorageProvider storage,
            IOptions<StorageOption> options)
        {
            _hub = hub;
            _logger = logger;
            _service = service;
            _storeService = storeService;
            _storage = storage;
            _sOptions = options.Value;
        }

        public async Task Handle(SynchroCloseCommand request, CancellationToken cancellationToken)
        {
            var data = await _service.GetOpenSynchroById(request.Id);
            if (data == null)
            {
                _logger.LogError("Synchronization data not found");
                try
                {
                    await _hub.OnNotifyWorkerStatus((int)HttpStatusCode.BadRequest,
                        $"Synchronization data  for id {request.Id} not found.", null);
                }
                catch
                {
                    // ignored
                }

                return;
            }

            // Mark closing
            _logger.LogInformation("Mark synchronization status = closing");
            data.Status = SynchroStatus.CLOSING;
            await _service.Put(data.Id, data);
            data.Status = SynchroStatus.CLOSE;

            var fileName = Path.GetTempPath() + Guid.NewGuid() + ".dat";
            _logger.LogInformation("Creating update file");

            var options = new JsonSerializerOptions
            {
                WriteIndented = false
            };

            SqlQueryBuilder builder = new SqlQueryBuilder();
            
            var store = await _storeService.GetWithTaxesById(data.StoreId);

            foreach (var detail in data.SynchroDetails)
            {
                //patch only between i fix the problem with the taxs on category.
                if (detail.Entity.Equals("LiteCategory"))
                {
                    //remove the tax not o the current store
                    var category =  JsonSerializer.Deserialize<LiteCategory>(detail.Data);
                    category.Taxes = category.Taxes.Where(x => store.Taxs.Any(t => t.Id == x)).ToList();
                    detail.Data = JsonSerializer.Serialize(category);
                }
                
                detail.ProcessDetail(ref builder);
            }

            // var jsonUtf8Bytes = JsonSerializer.SerializeToUtf8Bytes(data, options);
            byte[] jsonUtf8Bytes = Encoding.UTF8.GetBytes(builder.SaveString("Partial", data.Tag.ToString()));

            // await File.WriteAllBytesAsync(fileName, GetZipArchive("backup.pbak", jsonUtf8Bytes), cancellationToken);
            await File.WriteAllBytesAsync(fileName, GetZipArchive("backup.pbak", jsonUtf8Bytes), cancellationToken);

            _logger.LogInformation("Local file successfully created on the path {Filename}", fileName);

            _logger.LogInformation("Uploading file in to S3");
            var enterprise = _sOptions.RootFolder;
            var folder = $"Clients/{enterprise}/backupfiles";

            var path = await _storage.Upload(folder, fileName,
                useSameFilename: $"parcial_backup_{data.StoreId}_{data.Tag}.gpb");
            if (path == null) return;
            data.FilePath = $"{folder}/{path}";
            await _service.Put(data.Id, data);
            //update las close synchro
            await _storeService.UpdateLastCloseSynchro(data.StoreId, data.Tag);

            _logger.LogInformation("Updating the synchronization and marking it as closed");

            try
            {
                await _hub.OnNotifyWorkerStatus((int)HttpStatusCode.OK,
                    $"Synchronization data  for id {request.Id} success.", data);
            }
            catch
            {
                // ignored
            }

            return;
        }

        private byte[] GetZipArchive(string filename, byte[] content)
        {
            using var archiveStream = new MemoryStream();
            using (var archive = new ZipArchive(archiveStream, ZipArchiveMode.Create, true))
            {
                var zipArchiveEntry = archive.CreateEntry(filename, CompressionLevel.Fastest);
                using (var zipStream = zipArchiveEntry.Open())
                {
                    zipStream.Write(content, 0, content.Length);
                }
            }

            var archiveFile = archiveStream.ToArray();

            return archiveFile;
        }
    }
}