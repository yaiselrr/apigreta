using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Amazon.S3.Model;
using AutoMapper.Internal;
using Greta.BO.Api.Abstractions;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Entities.Attributes;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Extensions;
using Greta.BO.BusinessLogic.Handlers.DataHandlers.Validations.Extensions;
using Greta.BO.BusinessLogic.Interfaces;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Models.Dto.ReportDto;
using Greta.BO.BusinessLogic.Models.Dto.Search;
using Greta.BO.BusinessLogic.Specifications.TimeKeepingSpecs;
using Greta.Sdk.EFCore.Extensions;
using LanguageExt;
using Microsoft.Azure.Amqp.Framing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Service;

///<inheritdoc/>
public interface ITimeKeepingService: IGenericBaseService<TimeKeeping>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="employeeId"></param>
    /// <param name="storeId"></param>
    /// <param name="date"></param>
    /// <returns></returns>
    Task<string> GetInformation(long employeeId, long storeId, DateTime date);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="employeeId"></param>
    /// <param name="employeeName"></param>
    /// <param name="deviceId"></param>
    /// <param name="storeId"></param>
    /// <param name="storeName"></param>
    /// <param name="date"></param>
    /// <param name="formatDate"></param>
    /// <returns></returns>
    Task<string> ClockIn(long employeeId, string employeeName, long? deviceId, long? storeId, string storeName,DateTime date, string formatDate);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="employeeId"></param>
    /// <param name="employeeName"></param>
    /// <param name="deviceId"></param>
    /// <param name="storeId"></param>
    /// <param name="storeName"></param>
    /// <param name="date"></param>
    /// <param name="formatDate"></param>
    /// <returns></returns>
    Task<string> ClockOut(long employeeId, string employeeName, long? deviceId, long? storeId, string storeName, DateTime date, string formatDate);

    /// <summary>
    /// Calculate WorkTimeReport
    /// </summary>
    /// <param name="storeId"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    Task<List<WorkTimeReportModel>> WorkTimeReport(long storeId, WorkTimeSearchModel filter);

    /// <summary>
    /// Export Csv
    /// </summary>
    /// <param name="workTimeReportModel"></param>
    /// <returns></returns>
    Task<string> ExportCsv(List<WorkTimeReportModel> workTimeReportModel);
}

/// <inheritdoc cref="ITimeKeepingService" />
public class TimeKeepingService: BaseService<ITimeKeepingRepository, TimeKeeping>, ITimeKeepingService
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="repository"></param>
    /// <param name="logger"></param>
    public TimeKeepingService(ITimeKeepingRepository repository, ILogger<TimeKeepingService> logger)
        : base(repository, logger)
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="employeeId"></param>
    /// <param name="storeId"></param>
    /// <param name="date"></param>
    /// <returns></returns>
    public async Task<string> GetInformation(long employeeId, long storeId, DateTime date)
    {
        //detect if the user has clocked in today
        var timeKeepings = await _repository.GetEntity<TimeKeeping>()
                        .Where(e => e.EmployeeId == employeeId &&
                                      e.BeginStoreId == storeId &&
                                      (e.Begin.Value.Date == date.ToUniversalTime().Date || e.End == null))
                        .ToListAsync();
        
        return timeKeepings.Count > 0 ? $"It has pending, do the clock out of the day: {date:MM/dd/yyyy}" : null;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="employeeId"></param>
    /// <param name="employeeName"></param>
    /// <param name="deviceId"></param>
    /// <param name="storeId"></param>
    /// <param name="storeName"></param>
    /// <param name="date"></param>
    /// <param name="formatDate"></param>
    /// <returns></returns>
    public async Task<string> ClockIn(long employeeId, string employeeName, long? deviceId, long? storeId, string storeName, DateTime date, string formatDate)
    {
        //if the same user has already clocked in this date, return an error
        var timeKeeping = await _repository.GetEntity<TimeKeeping>()
            .FirstOrDefaultAsync(e => e.EmployeeId == employeeId &&
                                      (e.Begin.Value.Date == date.Date && e.End == null));
        if (timeKeeping != null)
        {
            _logger.LogError("The user has already clocked in this date");
            return "The user has already clocked in this date";
        }
        
        timeKeeping = new TimeKeeping
        {
            Begin = date,
            EmployeeId = employeeId,
            EmployeeName = employeeName,
            BeginFormatDate = formatDate,
            BeginDeviceId = deviceId,
            BeginStoreName = storeName,
            BeginStoreId = storeId
        };
        await _repository.CreateAsync(timeKeeping);
        return null;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="employeeId"></param>
    /// <param name="employeeName"></param>
    /// <param name="deviceId"></param>
    /// <param name="storeId"></param>
    /// <param name="storeName"></param>
    /// <param name="date"></param>
    /// <param name="formatDate"></param>
    /// <returns></returns>
    public async Task<string> ClockOut(long employeeId, string employeeName, long? deviceId, long? storeId, string storeName, DateTime date, string formatDate)
    {
        //if the same user has already clocked out this date, return an error
        var timeKeeping = await _repository.GetEntity<TimeKeeping>()
            .FirstOrDefaultAsync(e => e.EmployeeId == employeeId &&
                                      (e.Begin.Value.Date == date.Date && e.End == null));
        if (timeKeeping != null)
        {
            //validate if clock out is less than clock in
            Debug.Assert(timeKeeping.Begin != null, "timeKeeping.Begin != null");
            if (timeKeeping.Begin.Value > date)
            {
                _logger.LogError("The clock out date cannot be less than the clock in date");
                return "The clock out date cannot be less than the clock in date";
            }
            
            //calculate the time worked
            var timeWorked = date - timeKeeping.Begin.Value;
            timeKeeping.TimeWorked = timeWorked.TotalHours;
            timeKeeping.TimeWorkedFormat = $"{timeWorked:hh} hours {timeWorked:mm} minutes {timeWorked:ss} seconds";

            timeKeeping.EndStoreName = storeName;
            timeKeeping.EndStoreId = storeId;
            timeKeeping.End = date;
            timeKeeping.EndFormatDate = formatDate;
            timeKeeping.EndDeviceId = deviceId;
            await _repository.UpdateAsync(timeKeeping);
            return null;
        }
        _logger.LogError("The user has not clocked in this date");
        return "The user has not clocked in this date";
    }

    /// <summary>
    /// Return list of time worked by store for all employees by day between dates
    /// </summary>
    /// <param name="storeId"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<List<WorkTimeReportModel>> WorkTimeReport(long storeId, WorkTimeSearchModel filter)
    {
        if (filter.Mode == Models.Enums.TimeKeepingUserFilterMode.Custom && 
            (filter.From == null || filter.To == null || filter.From >= filter.To))
        {   
            _logger.LogError("The range of date is invalid");
            throw new BusinessLogicException("The range of date is invalid");
        }

        if (filter.Mode == Models.Enums.TimeKeepingUserFilterMode.Week)
        {
            DateTime datetime = DateTime.UtcNow.StartOfWeek();
            var lastSunday = new DateTime(datetime.Year, datetime.Month, datetime.Day, 0, 0, 0, DateTimeKind.Utc);
            var endDate = lastSunday.AddDays(7);
            filter.From = lastSunday;
            filter.To = endDate;
        }

        if (filter.Mode == Models.Enums.TimeKeepingUserFilterMode.Month)
        {
            DateTime datetime = DateTime.UtcNow;
            var firstDayOfMonth = new DateTime(datetime.Year, datetime.Month, 1, 0, 0, 0, DateTimeKind.Utc);
            var lastDayOfMonthTemp = firstDayOfMonth.AddMonths(1).AddDays(-1);
            var lastDayOfMonth = new DateTime(lastDayOfMonthTemp.Year, datetime.Month, lastDayOfMonthTemp.Day, 23, 59, 59, DateTimeKind.Utc);
            filter.From = firstDayOfMonth;
            filter.To = lastDayOfMonth;
        }

        var entities = await _repository.GetEntity<TimeKeeping>()
                                        .WithSpecification(new WorkTimeFilterSpec(storeId, filter))                                           
                                        .ToListAsync();
                
        var timeKeeping = entities.GroupBy(x => x.EmployeeName);

        List<WorkTimeReportModel> workTimeReportModels = new List<WorkTimeReportModel>();

        foreach (var item in timeKeeping)
        {
            var groupbyWeek = item.GroupBy(x => x.Week).OrderBy(x=>x.Key);

            foreach (var week in groupbyWeek)
            {
                var anyDayOfWeek = week.FirstOrDefault(x => x.Begin != null).Begin;
                var startWeek = ((DateTime)anyDayOfWeek).StartOfWeek();

                WorkTimeReportModel workTimeReportModel = new WorkTimeReportModel();
                workTimeReportModel.EmployeeName = item.Key.ToString();
                workTimeReportModel.EmployeeId = item.First().EmployeeId;

                // Fill time worked in hours

                if (IsDateInRange(startWeek, (DateTime)filter.From, (DateTime)filter.To))
                {
                    var wtSundayValue = week.Where(x => x.Begin.Value.DayOfWeek == DayOfWeek.Sunday);
                    if (wtSundayValue.Count() > 0)
                        workTimeReportModel.WtSundayValue = wtSundayValue.Sum(x => x.TimeWorked);
                    else
                        workTimeReportModel.WtSundayValue = 0;
                }
                else
                {
                    workTimeReportModel.WtSundayValue = -1;
                }
                                           
                if (IsDateInRange(startWeek.AddDays(1), (DateTime)filter.From, (DateTime)filter.To))
                {
                    var wtMondayValue = week.Where(x => x.Begin.Value.DayOfWeek == DayOfWeek.Monday);
                    if (wtMondayValue.Count() > 0)
                        workTimeReportModel.WtMondayValue = wtMondayValue.Sum(x => x.TimeWorked);
                    else
                        workTimeReportModel.WtMondayValue = 0;
                }
                else
                {
                    workTimeReportModel.WtMondayValue = -1;
                }

                if (IsDateInRange(startWeek.AddDays(2), (DateTime)filter.From, (DateTime)filter.To))
                {
                    var wtTuesdayValue = week.Where(x => x.Begin.Value.DayOfWeek == DayOfWeek.Tuesday);
                    if (wtTuesdayValue.Count() > 0)
                        workTimeReportModel.WtTuesdayValue = wtTuesdayValue.Sum(x => x.TimeWorked);
                    else
                        workTimeReportModel.WtTuesdayValue = 0;
                }
                else
                {
                    workTimeReportModel.WtTuesdayValue = -1;
                }

                if (IsDateInRange(startWeek.AddDays(3), (DateTime)filter.From, (DateTime)filter.To))
                {
                    var wtWednesdayValue = week.Where(x => x.Begin.Value.DayOfWeek == DayOfWeek.Wednesday);
                    if (wtWednesdayValue.Count() > 0)
                        workTimeReportModel.WtWednesdayValue = wtWednesdayValue.Sum(x => x.TimeWorked);
                    else
                        workTimeReportModel.WtWednesdayValue = 0;
                }
                else
                {
                    workTimeReportModel.WtWednesdayValue = -1;
                }

                if (IsDateInRange(startWeek.AddDays(4), (DateTime)filter.From, (DateTime)filter.To))
                {
                    var wtThursdayValue = week.Where(x => x.Begin.Value.DayOfWeek == DayOfWeek.Thursday);
                    if (wtThursdayValue.Count() > 0)
                        workTimeReportModel.WtThursdayValue = wtThursdayValue.Sum(x => x.TimeWorked);
                    else
                        workTimeReportModel.WtThursdayValue = 0;
                }
                else
                {
                    workTimeReportModel.WtThursdayValue = -1;
                }

                if (IsDateInRange(startWeek.AddDays(5), (DateTime)filter.From, (DateTime)filter.To))
                {
                    var wtFridayValue = week.Where(x => x.Begin.Value.DayOfWeek == DayOfWeek.Friday);
                    if (wtFridayValue.Count() > 0)
                        workTimeReportModel.WtFridayValue = wtFridayValue.Sum(x => x.TimeWorked);
                    else
                        workTimeReportModel.WtFridayValue = 0;
                }
                else
                {
                    workTimeReportModel.WtFridayValue = -1;
                }

                if (IsDateInRange(startWeek.AddDays(6), (DateTime)filter.From, (DateTime)filter.To))
                {
                    var wtSaturdayValue = week.Where(x => x.Begin.Value.DayOfWeek == DayOfWeek.Saturday);
                    if (wtSaturdayValue.Count() > 0)
                        workTimeReportModel.WtSaturdayValue = wtSaturdayValue.Sum(x => x.TimeWorked);
                    else
                        workTimeReportModel.WtSaturdayValue = 0;
                }
                else
                {
                    workTimeReportModel.WtSaturdayValue = -1;
                }

                //Fill string time worked
                if (workTimeReportModel.WtSundayValue > -1)
                {
                    TimeSpan SundayFromHours = TimeSpan.FromHours(workTimeReportModel.WtSundayValue);
                    workTimeReportModel.WtSunday = $"{SundayFromHours:hh} hours {SundayFromHours:mm} minutes {SundayFromHours:ss} seconds";
                }
                else
                {
                    workTimeReportModel.WtSunday = "-";
                }

                if (workTimeReportModel.WtMondayValue > -1)
                {
                    TimeSpan MonadyFromHours = TimeSpan.FromHours(workTimeReportModel.WtMondayValue);
                    workTimeReportModel.WtMonday = $"{MonadyFromHours:hh} hours {MonadyFromHours:mm} minutes {MonadyFromHours:ss} seconds";
                }
                else
                {
                    workTimeReportModel.WtMonday = "-";
                }

                if (workTimeReportModel.WtTuesdayValue > -1)
                {
                    TimeSpan TuesdayFromHours = TimeSpan.FromHours(workTimeReportModel.WtTuesdayValue);
                    workTimeReportModel.WtTuesday = $"{TuesdayFromHours:hh} hours {TuesdayFromHours:mm} minutes {TuesdayFromHours:ss} seconds";
                }
                else
                {
                    workTimeReportModel.WtTuesday = "-";
                }

                if (workTimeReportModel.WtWednesdayValue > -1)
                {
                    TimeSpan WednesdayFromHours = TimeSpan.FromHours(workTimeReportModel.WtWednesdayValue);
                    workTimeReportModel.WtWednesday = $"{WednesdayFromHours:hh} hours {WednesdayFromHours:mm} minutes {WednesdayFromHours:ss} seconds";
                }
                else
                {
                    workTimeReportModel.WtWednesday = "-";
                }

                if (workTimeReportModel.WtThursdayValue > -1)
                {
                    TimeSpan ThursdayFromHours = TimeSpan.FromHours(workTimeReportModel.WtThursdayValue);
                    workTimeReportModel.WtThursday = $"{ThursdayFromHours:hh} hours {ThursdayFromHours:mm} minutes {ThursdayFromHours:ss} seconds";
                }
                else
                {
                    workTimeReportModel.WtThursday = "-";
                }


                if (workTimeReportModel.WtFridayValue > -1)
                {
                    TimeSpan FridayFromHours = TimeSpan.FromHours(workTimeReportModel.WtFridayValue);
                    workTimeReportModel.WtFriday = $"{FridayFromHours:hh} hours {FridayFromHours:mm} minutes {FridayFromHours:ss} seconds";
                }
                else
                {
                    workTimeReportModel.WtFriday = "-";
                }

                if (workTimeReportModel.WtSaturdayValue > -1)
                {
                    TimeSpan SaturdayFromHours = TimeSpan.FromHours(workTimeReportModel.WtSaturdayValue);
                    workTimeReportModel.WtSaturday = $"{SaturdayFromHours:hh} hours {SaturdayFromHours:mm} minutes {SaturdayFromHours:ss} seconds";
                }
                else
                {
                    workTimeReportModel.WtSaturday = "-";
                }


                //Fill Total hours by week

                TimeSpan timeWorkedWeek = new TimeSpan(0,0,0);

                foreach (var day in week)
                {                   
                    if (day.End != null && day.Begin != null)
                    {
                        timeWorkedWeek += (DateTime)day.End - (DateTime)day.Begin;
                    }                    
                }

                workTimeReportModel.WtWeekValue = timeWorkedWeek.TotalHours;
                workTimeReportModel.WtWeek = $"{timeWorkedWeek:hh} hours {timeWorkedWeek:mm} minutes {timeWorkedWeek:ss} seconds";

                //Fill start and end week

                workTimeReportModel.WeekStart = startWeek;
                workTimeReportModel.WeekEnd = startWeek.AddDays(6);
                workTimeReportModel.NumberWeek = week.Key;
                workTimeReportModels.Add(workTimeReportModel);

            }
        }

        return workTimeReportModels.ToList();
    }

    static bool IsDateInRange(DateTime date, DateTime from, DateTime to)
    {  
        if (date.Day >= from.Day && date.Month >= from.Month &&
            date.Day <= to.Day && date.Month <= to.Month)
        {
            return true;
        }

        return false;
    }
        

    /// <summary>
    /// Export worked time to csv
    /// </summary>
    /// <param name="workTimeReportModel"></param>
    /// <returns></returns>
    /// <exception cref="BusinessLogicException"></exception>
    public Task<string> ExportCsv(List<WorkTimeReportModel> workTimeReportModel)
    {
        if (workTimeReportModel == null || workTimeReportModel.Count() == 0)
        {
            _logger.LogError("Invalid list of WorkTimeReport");
            throw new BusinessLogicException("Invalid or empty list of WorkTimeModelReport");
        }

        var fieldInfo = WorkTimeReportModel.GetFieldInfo<WorkTimeReportModel>().ToArray();

        var stringBuilder = new StringBuilder();

        // Generate header
        stringBuilder.AppendLine(string.Join(",", fieldInfo.Select(info => info.property.Name)));

        // generate rows
        PropertyInfo lastPropertyInfo = fieldInfo.Last().property;
        foreach (WorkTimeReportModel row in workTimeReportModel)
        {
            foreach ((PropertyInfo currentProperty, FieldInfoAttribute field) in fieldInfo)
            {
                stringBuilder.Append(string.Format(currentProperty.GetValue(row).ToString()));
                if (lastPropertyInfo != currentProperty) stringBuilder.Append(',');
            }
            stringBuilder.Append(Environment.NewLine);
        }

        return Task.FromResult(stringBuilder.ToString());
    }
}