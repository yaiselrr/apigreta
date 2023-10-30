using System;
using System.Threading;
using System.Threading.Tasks;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;

namespace Greta.BO.BusinessLogic.Handlers.Command.TimeKeeping;

/// <summary>
/// ClockInCommand
/// </summary>
/// <param name="EmployeeId"></param>
/// <param name="EmployeeName"></param>
/// <param name="DeviceLicenceCode"></param>
/// <param name="Date"></param>
/// <param name="FormatDate"></param>
public record ClockInCommand(long EmployeeId, string EmployeeName, string DeviceLicenceCode, DateTime Date, string FormatDate): IRequest<ClockInResponse>;
/// <summary>
/// ClockInResponse
/// </summary>
public record ClockInResponse: CQRSResponse<string>;

/// <summary>
/// ClockInHandler
/// </summary>
public class ClockInHandler: IRequestHandler<ClockInCommand, ClockInResponse>
{
    private readonly ITimeKeepingService _timeKeepingService;
    private readonly IDeviceService _deviceService;

    /// <summary>
    /// ClockInHandler
    /// </summary>
    /// <param name="timeKeepingService"></param>
    /// <param name="deviceService"></param>
    public ClockInHandler(
        ITimeKeepingService timeKeepingService,
        IDeviceService deviceService
        )
    {
        _timeKeepingService = timeKeepingService;
        _deviceService = deviceService;
    }

    /// <inheritdoc />
    public async Task<ClockInResponse> Handle(ClockInCommand request, CancellationToken cancellationToken)
    {
        var device = await _deviceService.GetByDeviceLic(request.DeviceLicenceCode);
        if (device == null)
        {
            return  new ClockInResponse() { Data = "Device not found" };
        }
        
        var response = await _timeKeepingService.ClockIn(
            request.EmployeeId,
            request.EmployeeName,
            device.Id,
            device.StoreId,
            device.Store.Name,
            request.Date,
            request.FormatDate
        );
        return new ClockInResponse() { Data = response };
    }
}