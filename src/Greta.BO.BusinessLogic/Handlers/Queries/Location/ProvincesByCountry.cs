using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Queries.Location
{
    public static class ProvincesByCountry
    {
        public record Query(long countryId) : IRequest<Response>;

        public class Handler : IRequestHandler<Query, Response>
        {
            protected readonly ILogger _logger;
            protected readonly IMapper _mapper;
            protected readonly ILocationService _service;

            public Handler(ILogger<Handler> logger, ILocationService service, IMapper mapper)
            {
                _logger = logger;
                _service = service;
                _mapper = mapper;
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                var entities = await _service.GetProvincesByCountry(request.countryId);
                return new Response {Data = _mapper.Map<List<ProvinceModel>>(entities)};
            }
        }

        public record Response : CQRSResponse<List<ProvinceModel>>;
    }
}