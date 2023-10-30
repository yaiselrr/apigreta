using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Authorization;
using MediatR;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using Microsoft.Extensions.Logging;
using System.Threading;

namespace Greta.BO.BusinessLogic.Handlers.Queries.ScaleReasonCodes
{
    public class ScaleReasonCodesGetAll
    {
        public record Query(bool ToExport = false) : IRequest<Response>, IAuthorizable
        {
            public List<IRequirement> Requirements => new()
            {
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
                var entities = await _service.Get();               
                
                return new Response { Data = _mapper.Map<List<ScaleReasonCodesModel>>(entities) };
            }
        }

        public record Response : CQRSResponse<List<ScaleReasonCodesModel>>;
        
    }
}
