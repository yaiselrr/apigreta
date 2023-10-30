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

namespace Greta.BO.BusinessLogic.Handlers.Command.VendorContact
{
    public static class VendorContactCreate
    {
        public record Command(VendorContactModel entity) : IRequest<Response>, IAuthorizable
        {
            public List<IRequirement> Requirements => new()
            {
                new PermissionRequirement.Requirement($"add_edit_vendor_contact")
            };
        }

        public class Validator : AbstractValidator<Command>
        {
            private readonly IVendorContactService _service;

            public Validator(IVendorContactService service)
            {
                _service = service;
                RuleFor(x => x.entity.Contact)
                    .NotEmpty()
                    .Length(3, 64);

                //RuleFor(x => x.entity.Contact, x => x.entity.Vendor)
                //        .MustAsync(ContactVendorUnique).WithMessage("VendorContact contact and vendor already exists.");
            }

            /*
            private async Task<bool> ContactVendorUnique(string contact,  CancellationToken cancellationToken)
            {
                var upcExist = await this._service.GetByContactVendor(contact,);
                return upcExist == null;
            }
            */
        }

        public class Handler : IRequestHandler<Command, Response>
        {
            protected readonly ILogger _logger;
            protected readonly IMapper _mapper;
            protected readonly IVendorContactService _service;

            public Handler(
                ILogger<Handler> logger,
                IVendorContactService service,
                IMapper mapper)
            {
                _logger = logger;
                _service = service;
                _mapper = mapper;
            }

            public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
            {
                var entity = _mapper.Map<Api.Entities.VendorContact>(request.entity);
                var result = await _service.Post(entity);
                _logger.LogInformation($"Create VendorContact {result.Contact} for user {result.UserCreatorId}");
                return new Response {Data = _mapper.Map<VendorContactModel>(result)};
            }
        }

        public record Response : CQRSResponse<VendorContactModel>;
    }
}