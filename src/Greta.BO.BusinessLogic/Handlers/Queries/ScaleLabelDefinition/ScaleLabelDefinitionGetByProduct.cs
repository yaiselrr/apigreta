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

namespace Greta.BO.BusinessLogic.Handlers.Queries.ScaleLabelDefinition
{
    public static class ScaleLabelDefinitionGetByProduct
    {
        public record Query(long Id) : IRequest<Response>, IAuthorizable, ICacheable
        {
            public List<IRequirement> Requirements => new() {
                new PermissionRequirement.Requirement($"associate_product_scale_label_definition")
            };

            public string CacheKey => $"ScaleLabelDefinitionGetById{Id}";
        }

        public class Handler : IRequestHandler<Query, Response>
        {
            protected readonly ILogger _logger;
            protected readonly IMapper _mapper;
            protected readonly IScaleLabelDefinitionService _service;

            public Handler(ILogger<Handler> logger, IScaleLabelDefinitionService service, IMapper mapper)
            {
                _logger = logger;
                _service = service;
                _mapper = mapper;
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                var entity = await _service.Get(request.Id);
                var data = _mapper.Map<ScaleLabelDefinitionModel>(entity);
                return data == null ? null : new Response {Data = data};
            }
        }

        public record Response : CQRSResponse<ScaleLabelDefinitionModel>;
    }
}