using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Models.Dto;
using MediatR;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Greta.BO.BusinessLogic.Service;
using Microsoft.Extensions.Logging;
using System.Threading;

namespace Greta.BO.BusinessLogic.Handlers.Command.ScaleReasonCodes
{
    public static class ScaleReasonCodesCreate
    {
        public record Command(ScaleReasonCodesModel entity) : IRequest<Response>, IAuthorizable
        {
            public List<IRequirement> Requirements => new()
            {
                new PermissionRequirement.Requirement($"add_edit_reason_codes")
            };
        }
        public class Validator : AbstractValidator<Command>
        {
            private readonly IScaleReasonCodesService _service;

            public Validator(IScaleReasonCodesService service)
            {
                _service = service;
                RuleFor(x => x.entity.Name)
                    .NotEmpty()
                    .Length(3, 64)
                    .MustAsync(NameUnique).WithMessage("ScaleReasonCodes name already exists.");
            }

            private async Task<bool> NameUnique(string name, CancellationToken cancellationToken)
            {
                var upcExist = await _service.GetByName(name);
                return upcExist == null;
            }
        }

        public class Handler : IRequestHandler<Command, Response>
        {
            protected readonly ILogger _logger;
            protected readonly IMapper _mapper;
            protected readonly IScaleReasonCodesService _service;

            public Handler(
                ILogger<Handler> logger,
                IScaleReasonCodesService service,
                IMapper mapper)
            {
                _logger = logger;
                _service = service;
                _mapper = mapper;
            }

            public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
            {
                var entity = _mapper.Map<Api.Entities.ScaleReasonCodes>(request.entity);
                var result = await _service.Post(entity);
                _logger.LogInformation($"Create ScaleReasonCodes {result.Name} for user {result.UserCreatorId}");
                return new Response { Data = _mapper.Map<ScaleReasonCodesModel>(result) };
            }
        }

        public record Response : CQRSResponse<ScaleReasonCodesModel>;
    }
}
