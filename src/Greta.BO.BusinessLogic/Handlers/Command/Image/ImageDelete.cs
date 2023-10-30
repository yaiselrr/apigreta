using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using Greta.Sdk.FileStorage.Interfaces;
using Greta.Sdk.FileStorage.Options;
using MediatR;
using Microsoft.Extensions.Options;

namespace Greta.BO.BusinessLogic.Handlers.Command.Image
{
    public static class ImageDelete
    {
        public record Command(long productImageId) : IRequest<Response>//, IAuthorizable
        {
            // public List<IRequirement> Requirements => new()
            // {
            //     new PermissionRequirement.Requirement($"delete_{nameof(Image).ToLower()}")
            // };
        }

        public class Handler : IRequestHandler<Command, Response>
        {
            private readonly IImageService _service;
            private readonly StorageOption _sOptions;
            private readonly IStorageProvider _storage;

            public Handler(IStorageProvider storage, IImageService service, IOptions<StorageOption> options)
            {
                _storage = storage;
                _service = service;
                _sOptions = options.Value;
            }

            public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
            {
                if (_sOptions == null) return new Response();
                var pI = await _service.GetImage(request.productImageId);
                if (pI == null) return new Response();

                var enterprise = _sOptions.RootFolder;
                var path = await _storage.DeleteFile(pI.Path);
                //     $"Clients/{enterprise}/{pI.Path}"
                // );
                var result = await _service.DeleteImage(request.productImageId);
                return new Response {Data = result};
            }
        }

        public record Response : CQRSResponse<bool>;
    }
}