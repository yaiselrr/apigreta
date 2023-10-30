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

namespace Greta.BO.BusinessLogic.Handlers.Command.StoreProduct
{
    public class StoreProductDeleteRange
    {
        public record Command(List<long> Ids) : IRequest<Response>, IAuthorizable
        {
            public List<IRequirement> Requirements => new()
            {
                new PermissionRequirement.Requirement($"associate_product_store")
            };
        }

        public class Handler : IRequestHandler<Command, Response>
        {
            protected readonly ILogger _logger;
            protected readonly IMapper _mapper;
            protected readonly IStoreProductService _service;

            public Handler(
                ILogger<Handler> logger,
                IStoreProductService service,
                IMapper mapper)
            {
                _logger = logger;
                _service = service;
                _mapper = mapper;
            }

            public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
            {
                var result = await _service.DeleteRange(request.Ids);
                _logger.LogInformation($"Entities with ids = {request.Ids} Deleted successfully.");
                return new Response {Data = result};
            }
        }

        public record Response : CQRSResponse<bool>;
    }
}