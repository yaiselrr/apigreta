using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Core.Caching;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Queries.ScaleHomeFav
{
    public static class ScaleHomeFavGetById
    {
        public record Query(long Id) : IRequest<Response>, IAuthorizable, ICacheable
        {
            public List<IRequirement> Requirements => new() {
                new PermissionRequirement.Requirement($"view_scale_home_fav")
            };

            public string CacheKey => $"ScaleHomeFavGetById{Id}";
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
                var entity = await _service.Get(request.Id);
                var data = _mapper.Map<ScaleHomeFavModel>(entity);
                return data == null ? null : new Response {Data = data};
            }
        }

        public record Response : CQRSResponse<ScaleHomeFavModel>;
    }
}