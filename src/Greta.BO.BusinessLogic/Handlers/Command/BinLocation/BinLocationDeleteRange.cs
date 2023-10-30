using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Command.BinLocation
{
    public class BinLocationDeleteRange
    {
        public record Command(List<long> Ids) : IRequest<Response>, IAuthorizable
        {
            public List<IRequirement> Requirements => new()
            {
                new PermissionRequirement.Requirement($"delete_bin_location")
            };
        }

        public class Handler : IRequestHandler<Command, Response>
        {
            protected readonly ILogger _logger;
            protected readonly IMapper _mapper;
            protected readonly IBinLocationService _service;

            public Handler(
                ILogger<Handler> logger,
                IBinLocationService service,
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