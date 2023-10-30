using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Authorization;
using MediatR;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using Microsoft.Extensions.Logging;
using System.Threading;

namespace Greta.BO.BusinessLogic.Handlers.Queries.ScaleReasonCodes
{
    public class ScaleReasonCodesGetById
    {
        public record Query(long Id) : IRequest<Response>, IAuthorizable
        {
            public string CacheKey => $"ScaleReasonCodesGetById{Id}";
            public List<IRequirement> Requirements => new() {
                new PermissionRequirement.Requirement($"view_reason_codes")
            };
        }
        public class Handler : IRequestHandler<Query, Response>
        {
            protected readonly ILogger _logger;
            protected readonly IMapper _mapper;
            protected readonly IScaleReasonCodesService _service;

            public Handler(ILogger<Handler> logger, IScaleReasonCodesService service, IMapper mapper)
            {
                _logger = logger;
                _service = service;
                _mapper = mapper;
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                var entity = await _service.Get(request.Id);
                var data = _mapper.Map<ScaleReasonCodesModel>(entity);
                return data == null ? null : new Response { Data = data };
            }
        }

        public record Response : CQRSResponse<ScaleReasonCodesModel>;

    }
}
