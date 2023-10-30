using System;
using System.Threading;
using System.Threading.Tasks;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;

namespace Greta.BO.BusinessLogic.Handlers.Command.TimeKeeping;

/// <summary>
/// ClockOutCommand
/// </summary>
/// <param name="EmployeeId"></param>
/// <param name="EmployeeName"></param>
/// <param name="DeviceLicenceCode"></param>
/// <param name="Date"></param>
/// <param name="FormatDate"></param>
public record ClockOutCommand(long EmployeeId, string EmployeeName, string DeviceLicenceCode, DateTime Date, string FormatDate): IRequest<ClockOutResponse>;
/// <summary>
/// ClockOutResponse
/// </summary>
public record ClockOutResponse: CQRSResponse<string>;

/// <summary>
/// ClockOutHandler
/// </summary>
public class ClockOutHandler: IRequestHandler<ClockOutCommand, ClockOutResponse>
{
    private readonly ITimeKeepingService _timeKeepingService;
    private readonly IDeviceService _deviceService;

    /// <summary>
    /// ClockOutHandler
    /// </summary>
    /// <param name="timeKeepingService"></param>
    public ClockOutHandler(ITimeKeepingService timeKeepingService,
        IDeviceService deviceService)
    {
        _timeKeepingService = timeKeepingService;
        _deviceService = deviceService;
    }

    /// <inheritdoc />
    public async Task<ClockOutResponse> Handle(ClockOutCommand request, CancellationToken cancellationToken)
    {
        var device = await _deviceService.GetByDeviceLic(request.DeviceLicenceCode);
        if (device == null)
        {
            return  new ClockOutResponse() { Data = "Device not found" };
        }
        
        var response = await _timeKeepingService.ClockOut(
            request.EmployeeId,
            request.EmployeeName,
            device.Id,
            device.StoreId,
            device.Store.Name,
            request.Date,
            request.FormatDate
        );
        return new ClockOutResponse() { Data = response };
    }
}