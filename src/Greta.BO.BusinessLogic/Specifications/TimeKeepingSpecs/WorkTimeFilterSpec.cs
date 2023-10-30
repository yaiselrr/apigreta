using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
public sealed class WorkTimeFilterSpec : Specification<TimeKeeping, TimeKeepingWorkTimeModel>
{
    /// <inheritdoc />
    public WorkTimeFilterSpec(long storeId, WorkTimeSearchModel filter)
    {
        Query.Where(x => x.BeginStoreId == storeId);        

        Query.Where(c => c.Begin.Value.Day >= filter.From.Value.Day && c.Begin.Value.Month >= filter.From.Value.Month &&
                             c.Begin.Value.Day <= filter.To.Value.Day && c.Begin.Value.Month <= filter.To.Value.Month);

        if (!string.IsNullOrEmpty(filter.Search))
        {
            Query.Where(x => x.EmployeeName.Equals(filter.Search));
        }

        Query.OrderBy(x => x.Begin);

        Query.Select(x => new TimeKeepingWorkTimeModel()
        {
            Begin = x.Begin,
            BeginDeviceId = x.BeginDeviceId,
            BeginFormatDate = x.BeginFormatDate,
            CreatedAt = x.CreatedAt,
            EmployeeId = x.EmployeeId,
            EmployeeName = x.EmployeeName,
            End = x.End,
            EndDeviceId = x.EndDeviceId,
            EndFormatDate = x.EndFormatDate,
            Id = x.Id,
            State = x.State,
            TimeWorked = x.TimeWorked,
            TimeWorkedFormat = x.TimeWorkedFormat,
            UpdatedAt = x.UpdatedAt,
            UserCreatorId = x.UserCreatorId,
            Week = CultureInfo.InvariantCulture.Calendar.GetWeekOfYear((DateTime)x.Begin, CalendarWeekRule.FirstFullWeek, DayOfWeek.Sunday)
        });
    }
}