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

namespace Greta.BO.BusinessLogic.Handlers.Queries.Rancher
{
    public static class RancherGetById
    {
        public record Query(long Id) : IRequest<Response>, IAuthorizable, ICacheable
        {
            public List<IRequirement> Requirements => new() {
                new PermissionRequirement.Requirement($"view_{nameof(Rancher).ToLower()}")
            };

            public string CacheKey => $"RancherGetById{Id}";
        }

        public class Handler : IRequestHandler<Query, Response>
        {
            protected readonly ILogger _logger;
            protected readonly IMapper _mapper;
            protected readonly IRancherService _service;

            public Handler(ILogger<Handler> logger, IRancherService service, IMapper mapper)
            {
                _logger = logger;
                _service = service;
                _mapper = mapper;
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                var entity = await _service.Get(request.Id);
                var data = _mapper.Map<RancherModel>(entity);
                return data == null ? null : new Response {Data = data};
            }
        }

        public record Response : CQRSResponse<RancherModel>;
    }
}