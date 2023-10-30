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

namespace Greta.BO.BusinessLogic.Handlers.Command.Vendor
{
    public class VendorUpdate
    {
        public record Command(long Id, VendorModel entity) : IRequest<Response>, IAuthorizable
        {
            public List<IRequirement> Requirements => new()
            {
                new PermissionRequirement.Requirement($"add_edit_{nameof(Vendor).ToLower()}")
            };
        }

        public class Validator : AbstractValidator<Command>
        {
            private readonly IVendorService _service;

            public Validator(IVendorService service)
            {
                _service = service;
                RuleFor(x => x.entity.Name)
                    .NotEmpty()
                    .Length(3, 64)
                    .MustAsync(NameUnique).WithMessage("Vendor name already exists.");
            }

            private async Task<bool> NameUnique(Command comand, string name, CancellationToken cancellationToken)
            {
                var upcExist = await _service.GetByName(name, comand.Id);
                return upcExist == null;
            }
        }

        public class Handler : IRequestHandler<Command, Response>
        {
            protected readonly ILogger _logger;
            protected readonly IMapper _mapper;
            protected readonly IVendorService _service;

            public Handler(
                ILogger<Handler> logger,
                IVendorService service,
                IMapper mapper)
            {
                _logger = logger;
                _service = service;
                _mapper = mapper;
            }

            public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
            {
                var entity = _mapper.Map<Api.Entities.Vendor>(request.entity);
                var success = await _service.Put(request.Id, entity);
                _logger.LogInformation($"Vendor {request.Id} update successfully.");
                return new Response {Data = success};
            }
        }

        public record Response : CQRSResponse<bool>;
    }
}