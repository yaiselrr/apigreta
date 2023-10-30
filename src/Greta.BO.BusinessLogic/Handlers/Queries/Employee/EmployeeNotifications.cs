using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Greta.BO.BusinessLogic.Extensions;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;

namespace Greta.BO.BusinessLogic.Handlers.Queries.Employee;

/// <summary>
/// Get All employee notifications 
/// </summary>
/// <param name="StoreId"></param>
/// <param name="EmployeeId"></param>
public record EmployeeNotificationsQuery(long StoreId, long EmployeeId) : IRequest<EmployeeNotificationsResponse>;
/// <summary>
/// Get All employee notifications response
/// </summary>
public record EmployeeNotificationsResponse: CQRSResponse<List<string>>;

/// <inheritdoc />
public class EmployeeNotificationsHandler: IRequestHandler<EmployeeNotificationsQuery, EmployeeNotificationsResponse>
{
    private readonly ISaleService _saleService;
    private readonly ITimeKeepingService _timeKeepingService;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="saleService"></param>
    /// <param name="timeKeepingService"></param>
    public EmployeeNotificationsHandler(ISaleService saleService, ITimeKeepingService timeKeepingService)
    {
        _saleService = saleService;
        _timeKeepingService = timeKeepingService;
    }
    
    public async Task<EmployeeNotificationsResponse> Handle(EmployeeNotificationsQuery request, CancellationToken cancellationToken)
    {
        var result = new List<string>();
        var startOfWeek = DateTime.Today.StartOfWeek();
        // var endOfWeek = startOfWeek.AddDays(7);
        
        //Get information about end of day not closed this week
        var initial = new DateTime(startOfWeek.Year, startOfWeek.Month, startOfWeek.Day, 0, 0, 0, 0);
        var datesNoEndOfDay = new List<string>();
        for (var i = 0; i < 7; i++)
        {
            var end = new DateTime(initial.Year, initial.Month, initial.Day, 23, 59, 59, 999);
            var list = await _saleService.GetCloseableElementsByStoreAndDate(request.StoreId, initial.ToUniversalTime(), end.ToUniversalTime());
            if (list.Any(x => x.Id == request.EmployeeId))
            {
                datesNoEndOfDay.Add(initial.ToString("MM/dd/yyyy"));
            }
            initial = initial.AddDays(1);
        }
        if (datesNoEndOfDay.Count > 0)
        {
            result.Add($"It has pending, do the end of day of the days: {string.Join(", ", datesNoEndOfDay)}");
        }
        //get information about TimeKeeping this week
        var initialClock = new DateTime(startOfWeek.Year, startOfWeek.Month, startOfWeek.Day, 0, 0, 0, 0);
        for (var i = 0; i < 7; i++)
        {
            var data = await _timeKeepingService.GetInformation(request.EmployeeId, request.StoreId, initialClock);
            if(data != null)
                result.Add(data);
            initialClock = initialClock.AddDays(1);
        }
        return new EmployeeNotificationsResponse() { Data = result };
    }
}