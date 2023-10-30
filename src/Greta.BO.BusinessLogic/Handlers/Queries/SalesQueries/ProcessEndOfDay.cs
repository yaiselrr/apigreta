using System;
using System.Threading;
using System.Threading.Tasks;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Queries.SalesQueries
{
    public static class ProcessEndOfDay
    {
        public record Query(ProcessEndOfDayRequest data) : IRequest<Response>;

        public record Response : CQRSResponse<ProcessEndOfDayResponse>;
        
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
                //build begin and end date for filter
                var splited = request.data.Date.Split("/");
                if (splited.Length != 3)
                {
                    throw new BusinessLogicException("Date format incorrect");
                }

                int year = 0, month = 0, day = 0;
                try
                {
                    year = int.Parse(splited[2]);
                    month = int.Parse(splited[0]);
                    day = int.Parse(splited[1]);
                }
                catch
                {
                    throw new BusinessLogicException("Date format incorrect");
                }

                var initial = new DateTime(year, month, day, 0, 0, 0, 0, DateTimeKind.Utc);
                var end = new DateTime(year, month, day, 23, 59, 59, 999, DateTimeKind.Utc);

                try
                {
                    var data = await _service.ProcessEndOfDay(request.data.StoreId, request.data.ElementId,
                        request.data.Holder, initial, end, request.data.Persist == 1);

                    return new Response() {Data = data};
                }
                catch(Exception e)
                {
                    _logguer.LogError(e, "Error to persist end of day.");
                    throw new BussinessValidationException("Error to persist end of day.");
                }
            }
        }
    }
}