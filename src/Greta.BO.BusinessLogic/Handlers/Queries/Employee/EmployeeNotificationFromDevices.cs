using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;

namespace Greta.BO.BusinessLogic.Handlers.Queries.Employee;

public record EmployeeNotificationFromDeviceQuery
    (string DeviceLicenceCode, long EmployeeId) : IRequest<EmployeeNotificationFromDeviceResponse>;

public record EmployeeNotificationFromDeviceResponse : CQRSResponse<List<string>>;

public class EmployeeNotificationFromDeviceHandler : IRequestHandler<EmployeeNotificationFromDeviceQuery,
    EmployeeNotificationFromDeviceResponse>
{
    private readonly IDeviceService _deviceService;
    private readonly IMediator _mediator;

    /// <summary>
    /// ClockInHandler
    /// </summary>
    /// <param name="deviceService"></param>
    /// <param name="mediator"></param>
    public EmployeeNotificationFromDeviceHandler(
        IDeviceService deviceService,
        IMediator mediator
    )
    {
        _deviceService = deviceService;
        _mediator = mediator;
    }

    public async Task<EmployeeNotificationFromDeviceResponse> Handle(EmployeeNotificationFromDeviceQuery request,
        CancellationToken cancellationToken = default)
    {
        var device = await _deviceService.GetByDeviceLic(request.DeviceLicenceCode);
        if (device == null)
        {
            return new EmployeeNotificationFromDeviceResponse() { Data = new List<string>() };
        }

        var result = await _mediator.Send(new EmployeeNotificationsQuery(
            device.StoreId,
            request.EmployeeId
        ), cancellationToken);
        
        return new EmployeeNotificationFromDeviceResponse() { Data = result.Data };
    }
}