

using Greta.BO.BusinessLogic.Service;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Greta.BO.BusinessLogic.Handlers.Command.Customer
{
    /// <summary>
    /// Update points entity
    /// </summary>
    public static class CustomerUpdatePoints
    {
        /// <summary>
        /// Update entity
        /// </summary>
        /// <param name="Customers">Entity id</param>
        public record Command(List<Api.Entities.Customer> Customers) : INotification;

        /// <inheritdoc />
        public class Handler : INotificationHandler<Command>
        {
            private readonly ILogger<Handler> _logger;
            private readonly ICustomerService _service;

            /// <summary>
            /// 
            /// </summary>
            /// <param name="logger"></param>
            /// <param name="customerService"></param>
            public Handler(ILogger<Handler> logger, ICustomerService customerService)
            {
                this._logger = logger;
                this._service = customerService;
            }
            /// <inheritdoc />
            public async Task Handle(Command request, CancellationToken cancellationToken)
            {
                _logger.LogInformation("Processing customers");
                await _service.UpdatePoints(request.Customers);
                _logger.LogInformation("Complete process on customers");
            }
        }
    }
}
