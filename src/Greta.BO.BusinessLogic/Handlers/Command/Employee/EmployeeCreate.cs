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

namespace Greta.BO.BusinessLogic.Handlers.Command.Employee
{
    public static class EmployeeCreate
    {
        public record Command(EmployeeModel entity) : IRequest<Response>, IAuthorizable
        {
            public List<IRequirement> Requirements => new()
            {
                new PermissionRequirement.Requirement($"add_edit_{nameof(Employee).ToLower()}")
            };
        }

        public class Validator : AbstractValidator<Command>
        {
            private readonly IEmployeeService _service;

            public Validator(IEmployeeService service)
            {
                _service = service;
                RuleFor(x => x.entity.FirstName)
                    .NotEmpty()
                    .Length(3, 64);

                RuleFor(x => x.entity.LastName)
                    .NotEmpty()
                    .Length(3, 64);

                RuleFor(x => x.entity.Phone)
                    .NotEmpty()
                    .Length(3, 20);

                RuleFor(x => x.entity.Email)
                    .NotEmpty()
                    .Length(3, 100);

                RuleFor(x => x.entity.Password)
                    .NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Command, Response>
        {
            protected readonly ILogger _logger;
            protected readonly IMapper _mapper;
            protected readonly IEmployeeService _service;

            public Handler(
                ILogger<Handler> logger,
                IEmployeeService service,
                IMapper mapper)
            {
                _logger = logger;
                _service = service;
                _mapper = mapper;
            }

            public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
            {
                var entity = _mapper.Map<Api.Entities.Employee>(request.entity);
                var result = await _service.Post(entity);
                _logger.LogInformation($"Create Employee {result.FirstName} for user {result.UserCreatorId}");
                return new Response {Data = _mapper.Map<EmployeeModel>(result)};
            }
        }

        public record Response : CQRSResponse<EmployeeModel>;
    }
}