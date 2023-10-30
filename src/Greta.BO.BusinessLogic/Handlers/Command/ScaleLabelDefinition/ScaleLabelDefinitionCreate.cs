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
    public static class ScaleLabelDefinitionCreate
    {
        public record Command(ScaleLabelDefinitionModel entity) : IRequest<Response>, IAuthorizable
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
                var result = await _service.Post(entity);
                _logger.LogInformation($"Create ScaleLabelDefinition for user {result.UserCreatorId}");
                return new Response {Data = _mapper.Map<ScaleLabelDefinitionModel>(result)};
            }
        }

        public record Response : CQRSResponse<ScaleLabelDefinitionModel>;
    }
}