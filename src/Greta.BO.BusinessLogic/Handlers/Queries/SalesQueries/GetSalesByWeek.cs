using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Queries.SalesQueries
{
    public static class GetSalesByWeek
    {
        public record Query(long StoreId) : IRequest<Response>;

        public record Response : CQRSResponse<SalesByHourResponse>;
        
        public class Handler:IRequestHandler<Query, Response>
        {
            private readonly ILogger<Handler> _logguer;
            private readonly ISaleService _service;

            public Handler(ILogger<Handler> logguer, ISaleService service)
            {
                _logguer = logguer;
                _service = service;
            }
            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                var list = await _service.GetSalesByWeek(request.StoreId);
                return new Response() {Data = list};
            }
        }
    }
}