using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Command.Rancher
{
    public static class RancherCreate
    {
        public record Command(RancherModel entity) : IRequest<Response>, IAuthorizable
        {
            public List<IRequirement> Requirements => new()
            {
                new PermissionRequirement.Requirement($"add_edit_{nameof(Rancher).ToLower()}")
            };
        }

        public class Validator : AbstractValidator<Command>
        {
            private readonly IRancherService _service;

            public Validator(IRancherService service)
            {
                _service = service;
                RuleFor(x => x.entity.Name)
                    .NotEmpty()
                    .Length(3, 64)
                    .MustAsync(NameUnique).WithMessage("Rancher name already exists.");
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
            protected readonly IRancherService _service;

            public Handler(
                ILogger<Handler> logger,
                IRancherService service,
                IMapper mapper)
            {
                _logger = logger;
                _service = service;
                _mapper = mapper;
            }

            public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
            {
                var entity = _mapper.Map<Api.Entities.Rancher>(request.entity);
                var result = await _service.Post(entity);
                _logger.LogInformation($"Create Rancher {result.Name} for user {result.UserCreatorId}");
                return new Response {Data = _mapper.Map<RancherModel>(result)};
            }
        }

        public record Response : CQRSResponse<RancherModel>;
    }
}