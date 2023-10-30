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

namespace Greta.BO.BusinessLogic.Handlers.Queries.BinLocation
{
    public static class BinLocationGetAll
    {
        public record Query(long? StoreId = null) : IRequest<Response>, IAuthorizable
        {
            public List<IRequirement> Requirements => new()
            {
                new PermissionRequirement.Requirement($"view_bin_location")
            };
        }

        public class Handler : IRequestHandler<Query, Response>
        {
            protected readonly ILogger _logger;
            protected readonly IMapper _mapper;
            protected readonly IBinLocationService _service;

            public Handler(ILogger<Handler> logger, IBinLocationService service, IMapper mapper)
            {
                _logger = logger;
                _service = service;
                _mapper = mapper;
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                if (request.StoreId.HasValue)
                {
                    var entities = await _service.GetByStore( request.StoreId.Value);
                    return new Response {Data = _mapper.Map<List<BinLocationModel>>(entities)};
                }
                else
                {
                   var entities = await _service.Get();
                    return new Response {Data = _mapper.Map<List<BinLocationModel>>(entities)};
                }
            }
        }

        public record Response : CQRSResponse<List<BinLocationModel>>;
    }
}