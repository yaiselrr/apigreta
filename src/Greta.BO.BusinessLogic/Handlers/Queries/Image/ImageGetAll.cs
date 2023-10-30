using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Queries.Image
{
    public static class ImageGetAll
    {
        public record Query(long OwnerId, ImageType Type) : IRequest<Response>//, IAuthorizable
        {
            // public List<IRequirement> Requirements => new()
            // {
            //     new PermissionRequirement.Requirement($"view_{nameof(Image).ToLower()}")
            // };
        }

        public class Handler : IRequestHandler<Query, Response>
        {
            private readonly ILogger _logger;
            private readonly IMapper _mapper;
            private readonly IImageService _service;

            public Handler(ILogger<Handler> logger, IImageService service, IMapper mapper)
            {
                _logger = logger;
                _service = service;
                _mapper = mapper;
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                var entities = await _service.Get(request.OwnerId, request.Type);
                return new Response {Data = _mapper.Map<List<ImageModel>>(entities)};
            }
        }

        public record Response : CQRSResponse<List<ImageModel>>;
    }
}