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
    public static class ClientApplicationById
    {
        public record Query(long Id) : IRequest<Response>, IAuthorizable
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
                var entities = await _service.GetById(request.Id);
                _logger.LogInformation($"Get clientApplication by id {request.Id}.");
                return entities == null ? null : new Response {Data = _mapper.Map<ClientApplicationModel>(entities)};
            }
        }

        public record Response : CQRSResponse<ClientApplicationModel>;
    }
}