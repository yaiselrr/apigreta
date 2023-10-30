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

namespace Greta.BO.BusinessLogic.Handlers.Queries.VendorContact
{
    public static class VendorContactGetByVendor
    {
        public record Query(long Id) : IRequest<Response>, IAuthorizable
        {
            public List<IRequirement> Requirements => new()
            {
                new PermissionRequirement.Requirement($"view_vendor_contact")
            };
        }

        public class Handler : IRequestHandler<Query, Response>
        {
            protected readonly ILogger _logger;
            protected readonly IMapper _mapper;
            protected readonly IVendorContactService _service;

            public Handler(ILogger<Handler> logger, IVendorContactService service, IMapper mapper)
            {
                _logger = logger;
                _service = service;
                _mapper = mapper;
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                var entity = await _service.GetByVendor(request.Id);
                var data = _mapper.Map<List<VendorContactModel>>(entity);
                return data == null ? null : new Response {Data = data};
            }
        }

        public record Response : CQRSResponse<List<VendorContactModel>>;
    }
}