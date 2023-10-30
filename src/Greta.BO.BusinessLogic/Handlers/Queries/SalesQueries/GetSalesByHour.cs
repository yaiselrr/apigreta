
using System.Threading;
using System.Threading.Tasks;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Queries.SalesQueries
{
    public record GetSalesByHourQuery(long StoreId) : IRequest<GetSalesByHourResponse>;

    public record GetSalesByHourResponse : CQRSResponse<SalesByHourResponse>;

    public class Handler : IRequestHandler<GetSalesByHourQuery, GetSalesByHourResponse>
    {
        private readonly ISaleService _service;
        private readonly IStoreService storeService;

        public Handler(
            ISaleService service,
            IStoreService storeService)
        {
            _service = service;
            this.storeService = storeService;
        }

        public async Task<GetSalesByHourResponse> Handle(GetSalesByHourQuery request,
            CancellationToken cancellationToken)
        {
            //Change this fot get by id if is required
            var store = await storeService.Get(request.StoreId);

            if (store == null)
            {
                throw new BusinessLogicException($"Store with id {request.StoreId} not found");
            }
            var beginHour = store.OpenTime.Hour;
            var endHour = store.ClosedTime.Hour;

            var list = await _service.GetSalesByHourAndStore(beginHour, endHour, store.Id, store.TimeZoneId);
            return new GetSalesByHourResponse() { Data = list };
        }
    }
}