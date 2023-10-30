using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Models.Dto;
using MediatR;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Greta.BO.BusinessLogic.Service;
using System.Threading;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Command.ScaleReasonCodes
{
    public class ScaleReasonCodesUpdate
    {
        public record Command(long Id, ScaleReasonCodesModel entity) : IRequest<Response>, IAuthorizable
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

            private async Task<bool> NameUnique(Command command, string name, CancellationToken cancellationToken)
            {
                var upcExist = await _service.GetByName(name, command.Id);
                return upcExist == null;
            }
        }

        public class Handler : IRequestHandler<Command, Response>
        {
            private readonly ILogger _logger;
            private readonly IMapper _mapper;
            private readonly IScaleReasonCodesService _service;

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
                var success = await _service.Put(request.Id, entity);
                _logger.LogInformation($"ScaleReasonCodes {request.Id} update successfully.");
                return new Response { Data = success };
            }
        }

        public record Response : CQRSResponse<bool>;
    }
}
