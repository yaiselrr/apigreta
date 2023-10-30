using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Queries.SalesQueries
{
    public static class GetEndOfDayResume
    {
        public record Query(GetEndOfDayresumeRequest data) : IRequest<Response>;

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

            private DateTime ProcessDay(string date, bool begin)
            {
                var splited = date.Split("/");
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

                return begin ? new DateTime(year, month, day, 0, 0, 0, 0, DateTimeKind.Utc):
                    new DateTime(year, month, day, 23, 59, 59, 999, DateTimeKind.Utc);
            }
            
            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {


                var initial = ProcessDay(request.data.InitialDate, true);
                var end = ProcessDay(request.data.EndDate, false);
                try
                {
                    var list = await _service.GetEndOfDayResume(request.data.StoreId, initial, end);

                    return new Response() { Data = list };
                }catch(BusinessLogicException e)
                {
                    return new Response() { Errors = new List<string>() { e.Message }, StatusCode = System.Net.HttpStatusCode.BadRequest };
                }
            }
        }
    }
}