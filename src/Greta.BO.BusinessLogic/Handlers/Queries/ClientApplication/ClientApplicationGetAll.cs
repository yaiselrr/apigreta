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

namespace Greta.BO.BusinessLogic.Handlers.Queries.ClientApplication
{
    public static class ClientApplicationGetAll
    {
        public record Query : IRequest<Response>, IAuthorizable
        {
            public List<IRequirement> Requirements => new()
            {
                new PermissionRequirement.Requirement($"view_{nameof(Profiles).ToLower()}")
            };
        }


        public class Handler : IRequestHandler<Query, Response>
        {
            protected readonly ILogger _logger;
            protected readonly IMapper _mapper;
            protected readonly IClientApplicationService _service;

            public Handler(
                ILogger<Handler> logger,
                IClientApplicationService service,
                IMapper mapper)
            {
                _logger = logger;
                _service = service;
                _mapper = mapper;
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                var entities = await _service.Get();
                _logger.LogInformation("GetAll clientApplications.");
                return new Response {Data = _mapper.Map<List<ClientApplicationModel>>(entities)};
            }
        }

        public record Response : CQRSResponse<List<ClientApplicationModel>>;
    }
}