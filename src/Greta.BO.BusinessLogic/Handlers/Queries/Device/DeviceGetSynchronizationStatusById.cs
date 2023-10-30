using System.Threading;
using System.Threading.Tasks;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Models.Enums;
using Greta.BO.BusinessLogic.Service;
using MediatR;

namespace Greta.BO.BusinessLogic.Handlers.Queries.Device
{
    /// <summary>
    /// Get entity device get synchronization status by id
    /// </summary>
    public static class DeviceGetSynchronizationStatusById
    {
        /// <summary>
        /// </summary>
        /// <param name="Id">Store Id</param>
        public record DeviceGetSynchronizationStatusByIdQuery(long Id) : IRequest<DeviceGetSynchronizationStatusByIdResponse>;

        /// <inheritdoc />
        public class DeviceGetSynchronizationStatusByIdHandler : IRequestHandler<DeviceGetSynchronizationStatusByIdQuery, DeviceGetSynchronizationStatusByIdResponse>
        {
            private readonly IDeviceService _service;

            /// <summary>
            /// 
            /// </summary>
            /// <param name="service"></param>
            public DeviceGetSynchronizationStatusByIdHandler(IDeviceService service)
            {
                _service = service;
            }

            /// <inheritdoc />
            public async Task<DeviceGetSynchronizationStatusByIdResponse> Handle(DeviceGetSynchronizationStatusByIdQuery request, CancellationToken cancellationToken)
            {
                var data = await _service.GetSynchronizationStatus(request.Id);
                return new DeviceGetSynchronizationStatusByIdResponse { Data = data };
            }
        }

        /// <inheritdoc />
        public record DeviceGetSynchronizationStatusByIdResponse : CQRSResponse<DeviceSynchronizationStatus>;
    }
}