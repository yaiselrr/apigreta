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

namespace Greta.BO.BusinessLogic.Handlers.Command.PriceBatch
{
    public class PriceBatchDelete
    {
        public record Command(long Id) : IRequest<Response>, IAuthorizable
        {
            public List<IRequirement> Requirements => new()
            {
                new PermissionRequirement.Requirement($"delete_price_batch")
            };
        }

        public class Handler : IRequestHandler<Command, Response>
        {
            protected readonly ILogger _logger;
            protected readonly IMapper _mapper;
            protected readonly IPriceBatchService _service;

            public Handler(ILogger<Handler> logger, IPriceBatchService service, IMapper mapper)
            {
                _logger = logger;
                _service = service;
                _mapper = mapper;
            }

            public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
            {
                var result = await _service.Delete(request.Id);
                _logger.LogInformation($"Entity with id {request.Id} Deleted successfully.");
                return new Response {Data = result};
            }
        }

        public record Response : CQRSResponse<bool>;
    }
}