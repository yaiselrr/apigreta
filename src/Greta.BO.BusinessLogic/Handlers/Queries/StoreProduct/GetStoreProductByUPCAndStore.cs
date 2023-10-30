using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Queries.StoreProduct
{
    public static class GetStoreProductByUPCAndStore
    {
        public record Query(long storeId, string upc): IRequest<Response>;
        public record Response: CQRSResponse<StoreProductMinimalModel>;
        public class Handler : IRequestHandler<Query, Response>
        {
            protected readonly ILogger _logger;
            protected readonly IMapper _mapper;
            protected readonly IStoreProductService _service;

            public Handler(ILogger<Handler> logger, IStoreProductService service, IMapper mapper)
            {
                _logger = logger;
                _service = service;
                _mapper = mapper;
            }
            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                var storeProduct = await _service.GetStoreProductByUPC(request.storeId, request.upc);
                var data = this._mapper.Map<StoreProductMinimalModel>(storeProduct);
                return new Response() { Data = data };
            }
        }
    }
}