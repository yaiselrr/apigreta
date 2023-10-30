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

namespace Greta.BO.BusinessLogic.Handlers.Command.ScaleLabelDefinition
{
    public class ScaleLabelDefinitionUpdate
    {
        public record Command(long Id, ScaleLabelDefinitionModel entity) : IRequest<Response>, IAuthorizable
        {
            public List<IRequirement> Requirements => new()
            {
                new PermissionRequirement.Requirement($"associate_product_scale_label_definition")
            };
        }

        public class Handler : IRequestHandler<Command, Response>
        {
            protected readonly ILogger _logger;
            protected readonly IMapper _mapper;
            protected readonly IScaleLabelDefinitionService _service;

            public Handler(
                ILogger<Handler> logger,
                IScaleLabelDefinitionService service,
                IMapper mapper)
            {
                _logger = logger;
                _service = service;
                _mapper = mapper;
            }

            public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
            {
                var entity = _mapper.Map<Api.Entities.ScaleLabelDefinition>(request.entity);
                if (entity.ScaleLabelType2Id == -1) entity.ScaleLabelType2Id = null;
                var success = await _service.Put(request.Id, entity);
                _logger.LogInformation($"ScaleLabelDefinition {request.Id} update successfully.");
                return new Response {Data = success};
            }
        }

        public record Response : CQRSResponse<bool>;
    }
}