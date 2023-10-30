using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Queries.ScaleHomeFav
{
    public static class ScaleHomeFavGetAll
    {
        public record Query : IRequest<Response>, IAuthorizable
        {
            public List<IRequirement> Requirements => new()
            {
                new PermissionRequirement.Requirement($"view_scale_home_fav")
            };
        }

        public class Handler : IRequestHandler<Query, Response>
        {
            protected readonly ILogger _logger;
            protected readonly IMapper _mapper;
            protected readonly IScaleHomeFavService _service;

            public Handler(ILogger<Handler> logger, IScaleHomeFavService service, IMapper mapper)
            {
                _logger = logger;
                _service = service;
                _mapper = mapper;
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                var entities = await _service.Get();
                return new Response {Data = _mapper.Map<List<ScaleHomeFavModel>>(entities)};
            }
        }

        public record Response : CQRSResponse<List<ScaleHomeFavModel>>;
    }
}