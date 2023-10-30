using Greta.BO.BusinessLogic.Service;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Greta.BO.BusinessLogic.Handlers.Queries.Device
{
    /// <summary>
    /// Get entity by id
    /// </summary>
    public static class GetDevicesConnectedByStore
    {
        /// <summary>
        /// </summary>
        /// <param name="StoreId">Store Id</param>
        public record DeviceGetDevicesConnectedByStoreQuery(long StoreId) : IRequest<List<Api.Entities.Device>>;

        /// <inheritdoc />
        public class GetDevicesConnectedByStoreHandler : IRequestHandler<DeviceGetDevicesConnectedByStoreQuery, List<Api.Entities.Device>>
        {
            private readonly IDeviceService _service;

            /// <summary>
            /// 
            /// </summary>
            /// <param name="service"></param>
            public GetDevicesConnectedByStoreHandler(
                IDeviceService service
                )
            {
                this._service = service;
            }

            /// <inheritdoc />
            public async Task<List<Api.Entities.Device>> Handle(DeviceGetDevicesConnectedByStoreQuery request, CancellationToken cancellationToken)
            {
                return await _service.GetDeviceConnectedByStore(request.StoreId);
            }
        }
    }
}
