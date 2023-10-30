using Greta.BO.BusinessLogic.Service;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Greta.BO.BusinessLogic.Handlers.Command
{
    public static class ProcessProductsQtyOnSales
    {
        public record Command(List<Api.Entities.StoreProduct> qtySales, long storeId) : INotification;
        public class Handler : INotificationHandler<Command>
        {
            private readonly ILogger<Handler> _logger;
            private readonly ISaleService _service;

            public Handler(ILogger<Handler> logger, ISaleService service)
            {
                this._logger = logger;
                this._service = service;
            }
            public async Task Handle(Command request, CancellationToken cancellationToken)
            {
                await _service.ProcessQty(request.qtySales, request.storeId);
            }
        }
    }
}
