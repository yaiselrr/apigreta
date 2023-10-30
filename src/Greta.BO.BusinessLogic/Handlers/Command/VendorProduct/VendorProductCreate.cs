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

namespace Greta.BO.BusinessLogic.Handlers.Command.VendorProduct
{
    public static class VendorProductCreate
    {
        public record Command(VendorProductModel entity) : IRequest<Response>, IAuthorizable
        {
            public List<IRequirement> Requirements => new()
            {
                new PermissionRequirement.Requirement($"associate_product_vendor")
            };
        }

        public class Handler : IRequestHandler<Command, Response>
        {
            protected readonly ILogger _logger;
            protected readonly IMapper _mapper;
            protected readonly IVendorProductService _service;

            public Handler(
                ILogger<Handler> logger,
                IVendorProductService service,
                IMapper mapper)
            {
                _logger = logger;
                _service = service;
                _mapper = mapper;
            }

            public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
            {
                var entity = _mapper.Map<Api.Entities.VendorProduct>(request.entity);
                var result = await _service.Post(entity);
                _logger.LogInformation($"Create VendorProduct for user {result.UserCreatorId}");
                return new Response {Data = _mapper.Map<VendorProductModel>(result)};
            }
        }

        public record Response : CQRSResponse<VendorProductModel>;
    }
}