using System;
using System.Collections.Generic;
using Ardalis.Specification;
using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Extensions;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Models.Dto.Search;
using Greta.BO.BusinessLogic.Models.Enums;

namespace Greta.BO.BusinessLogic.Specifications.TimeKeepingSpecs;

/// <summary>
/// Filter the Time Keeping information for one user
/// </summary>
public sealed class TimeKeepingFilterSpec: Specification<TimeKeeping, TimeKeepingModel>
{
    /// <inheritdoc />
    public TimeKeepingFilterSpec(TimeKeepingUserSearchModel filter)
    {
        
        Query.Where(c => c.EmployeeId == filter.EmployeeId);
       
        if (!string.IsNullOrEmpty(filter.Search))
            Query.Search(c => c.EmployeeName, $"%{filter.Search}%");


        if (filter.Mode == TimeKeepingUserFilterMode.Week)
        {
            DateTime datetime = DateTime.UtcNow.StartOfWeek();
            var lastSunday = new DateTime(datetime.Year, datetime.Month, datetime.Day, 0, 0, 0, DateTimeKind.Utc);
            var endDate = lastSunday.AddDays(7);
            
            Query.Where(c => c.Begin >= lastSunday && c.Begin < endDate);
        }else if (filter.Mode == TimeKeepingUserFilterMode.Month)
        {
            DateTime datetime = DateTime.UtcNow;
            var firstDayOfMonth = new DateTime(datetime.Year, datetime.Month, 1, 0, 0, 0, DateTimeKind.Utc);
            var lastDayOfMonthTemp = firstDayOfMonth.AddMonths(1).AddDays(-1);
            var lastDayOfMonth = new DateTime(lastDayOfMonthTemp.Year, datetime.Month, 1, 0, 0, 0, DateTimeKind.Utc);
            
            Query.Where(c => c.Begin >= firstDayOfMonth && c.Begin < lastDayOfMonth);
        }
        else
        {
            Query.Where(c => c.Begin >= filter.From && c.Begin < filter.To);
        }
        
        var splited = string.IsNullOrEmpty(filter.Sort) ? new[] {"", ""} : filter.Sort.Split("_");

        ((List<OrderExpressionInfo<TimeKeeping>>)OrderExpressions).Add(new OrderExpressionInfo<TimeKeeping>(
            splited[0] switch
            {
                "employeeName" => f => f.EmployeeName,
                "begin" => f => f.Begin,
                "end" => f => f.End,
                "timeWorked" => f => f.TimeWorked,
                _ => f => f.Begin
            }, 
            splited[1] == "desc" ? OrderTypeEnum.OrderByDescending: OrderTypeEnum.OrderBy
        ));

        Query.Select(x => new TimeKeepingModel
        {
            Begin = x.Begin,
            BeginFormatDate = x.BeginFormatDate,
            End = x.End,
            EndFormatDate = x.EndFormatDate,
            EmployeeId = x.EmployeeId,
            EmployeeName = x.EmployeeName,
            EndDeviceId = x.EndDeviceId,
            BeginDeviceId = x.BeginDeviceId,
            TimeWorked = x.TimeWorked,
            TimeWorkedFormat = x.TimeWorkedFormat,
            Id = x.Id,
            State = x.State,
            UserCreatorId = x.UserCreatorId,
            CreatedAt = x.CreatedAt,
            UpdatedAt = x.UpdatedAt
        });

    }
}