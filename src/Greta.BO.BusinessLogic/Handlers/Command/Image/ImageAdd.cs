using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using Greta.Sdk.FileStorage.Interfaces;
using Greta.Sdk.FileStorage.Options;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Greta.BO.BusinessLogic.Handlers.Command.Image
{
    public static class ImageAdd
    {
        public record Command(long OwnerId, int IsPrimary, ImageType ImageType, IFormFile File) : IRequest<Response>//, IAuthorizable
        {
            // public List<IRequirement> Requirements => new()
            // {
            //     new PermissionRequirement.Requirement($"add_edit_{nameof(Image).ToLower()}")
            // };
        }

        public class Handler : IRequestHandler<Command, Response>
        {
            private readonly IImageService service;
            private readonly StorageOption sOptions;
            private readonly IStorageProvider storage;

            public Handler(IStorageProvider storage, IImageService service, IOptions<StorageOption> options)
            {
                this.storage = storage;
                this.service = service;
                sOptions = options.Value;
            }

            public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
            {
                if (sOptions == null) return new Response();
                var enterprise = sOptions.RootFolder;
                if (request.IsPrimary == 0)
                {
                    //pos image image type
                    var image = await service.GetPrimaryImage(request.ImageType, request.OwnerId);
                    if (image != null) await service.DeleteImage(image.Id);
                    var folder = Api.Entities.Image.getStorageFolder(request.ImageType);
                    var path = await storage.UploadImage($"{enterprise}/{folder}", request.File);
                    var pI = await service.AddImage(new Api.Entities.Image
                    {
                        Path = folder + "/" + path,
                        IsPrimary = true,
                        OwnerId = request.OwnerId,
                        Type = request.ImageType
                    });
                    return new Response {Data = pI};
                }
                else
                {
                    var folder = Api.Entities.Image.getStorageFolder(request.ImageType);
                    var path = await storage.UploadImage(
                        $"Clients/{enterprise}/{Api.Entities.Image.getStorageFolder(request.ImageType)}", request.File);
                    if (path == null)
                        return new Response
                        {
                            StatusCode = HttpStatusCode.PreconditionFailed,
                            Errors = new List<string> {"Storage action fail."}
                        };
                    var pI = await service.AddImage(new Api.Entities.Image
                    {
                        Path = folder + "/" + path,
                        IsPrimary = false,
                        OwnerId = request.OwnerId,
                        Type = request.ImageType
                    });
                    return new Response {Data = pI};
                }
            }
        }

        public record Response : CQRSResponse<Api.Entities.Image>;
    }
}