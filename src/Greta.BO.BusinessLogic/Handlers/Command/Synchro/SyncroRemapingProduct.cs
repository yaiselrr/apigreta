using System;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.BusinessLogic.Service;
using MediatR;
using Microsoft.Extensions.Logging;


namespace Greta.BO.BusinessLogic.Handlers.Command.Synchro
{
    /// <summary>
    /// This Handler allow to remap all products on database to d devices
    /// </summary>
    public static class SyncroRemapingProduct
    {
         public record Command(Guid StoreId) : IRequest;

        public class Handler : IRequestHandler<Command>
        {
            private readonly ILogger _logger;
            private readonly ISynchroService _synchroService;
            private readonly ISynchroService _service;
            private readonly IStoreService _storeService;

            public Handler(
                IStoreService storeService,
                ISynchroService service,
                ILogger<Handler> logger,
                ISynchroService synchroService
            )
            {
                _storeService = storeService;
                _logger = logger;
                _synchroService = synchroService;

                _service = service;
            }

            public async Task Handle(Command request, CancellationToken cancellationToken)
            {
                try
                {
                    _logger.LogInformation("Getting store information");
                    var store = await _storeService.GetByGuid(request.StoreId);
                    if (store == null)
                    {
                        _logger.LogError($"Failed to create full backup {request.StoreId} store not found");
                        return ;
                    }
                    _logger.LogInformation("Getting the data from the database");

                    var data = await _service.CreateAFullBackupFile(store.Id);
                    _logger.LogInformation("Data from the database successfully obtained");

                    foreach (var p in data.Products)
                    {
                        await _synchroService.AddSynchroToStore(
                            store.Id,
                            p,
                            SynchroType.UPDATE
                        );
                    }
                    foreach (var p in data.ScaleProducts)
                    {
                        await _synchroService.AddSynchroToStore(
                            store.Id,
                            p,
                            SynchroType.UPDATE
                        );
                    }
                    foreach (var p in data.KitProducts)
                    {
                        await _synchroService.AddSynchroToStore(
                            store.Id,
                            p,
                            SynchroType.UPDATE
                        );
                    }
                    
                    
                 
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Failed to create full backup");
                }

                return ;
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
}