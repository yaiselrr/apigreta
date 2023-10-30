using Greta.BO.BusinessLogic.Exceptions;
using Greta.Sdk.FileStorage.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Greta.BO.BusinessLogic.Handlers.Command.Auth;

namespace Greta.BO.BusinessLogic.Handlers.Command.Download
{
    public static class DownloadFile
    {
        public record Query(string Hash, string Password = null) : IRequest<byte[]>;

        public class Handler : IRequestHandler<Query, byte[]>
        {
            protected readonly ILogger _logger;
            private readonly IStorageProvider storage;
            private readonly IMediator _mediator;

            private readonly List<string> _restrictedAssets = new()
            {
                "assets/TS5TouchScaleTechnicalManual.pdf"
            };

            public Handler(ILogger<Handler> logger, IStorageProvider storage, IMediator mediator)
            {
                _logger = logger;
                this.storage = storage;
                _mediator = mediator;
            }

            public async Task<byte[]> Handle(Query request, CancellationToken cancellationToken)
            {
                try
                {
                    var hash = System.Text.Encoding.UTF8.GetString(
                        System.Convert.FromBase64String(HttpUtility.HtmlDecode(request.Hash)));
                    if (request.Password != null)
                    {
                        var checkPass = await _mediator.Send(new CheckAdminPasswordCommand(request.Password));
                        if (_restrictedAssets.Contains(hash) && !checkPass.Data)
                        {
                            throw new BusinessLogicException("Access Denied.");
                        }
                    }
                    _logger.LogInformation("Begin Download");
                    var data = await storage.Download(hash);
                    _logger.LogInformation("Finish Download");
                    return data;
                }
                catch (Exception e)
                {
                    throw new BussinessValidationException(new List<string>() { $"Error download file. {e.Message}{e.InnerException?.Message}" });
                }
            }

        }
    }
}