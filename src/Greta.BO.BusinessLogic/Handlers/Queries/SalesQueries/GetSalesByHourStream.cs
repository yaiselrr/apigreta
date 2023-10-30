
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Queries.SalesQueries
{
    public record GetSalesByHourStream(long StoreId) : IStreamRequest<GetSalesByHourResponse>;

    // public record GetSalesByHourStreamResponse : CQRSResponse<GetSalesByHourResponse>;

    public class GetSalesByHourStreamHandler : IStreamRequestHandler<GetSalesByHourStream, GetSalesByHourResponse>
    {
        private readonly ILogger<GetSalesByHourStreamHandler> _logguer;
        private readonly ISaleService _service;
        private readonly IMediator _mediator;
        private readonly IStoreService _storeService;

        public GetSalesByHourStreamHandler(ILogger<GetSalesByHourStreamHandler> logguer,
            ISaleService service,
            IMediator mediator,
            IStoreService storeService)
        {
            _logguer = logguer;
            _service = service;
            _mediator = mediator;
            this._storeService = storeService;
        }

        public async IAsyncEnumerable<GetSalesByHourResponse> Handle(
            GetSalesByHourStream request, 
            [EnumeratorCancellation] CancellationToken cancellationToken)
        {

            while (!cancellationToken.IsCancellationRequested)
            {
                yield return await _mediator.Send(new GetSalesByHourQuery(request.StoreId), cancellationToken);
                await Task.Delay(1000, cancellationToken);
            }
        }
    }
}