using AutoMapper;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Greta.BO.BusinessLogic.Handlers.Queries.BinLocation
{
    public static class GetProductByUPC
    {
        public record Query(long storeId, string upc): IRequest<Response>;
        public record Response: CQRSResponse<ProductInventoryModel>;
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
                var data = await _service.GetProductByUPC(request.storeId, request.upc);
                return new Response() { Data = this._mapper.Map<ProductInventoryModel>(data) };
            }
        }
    }
}
