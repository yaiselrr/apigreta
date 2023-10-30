using System;
using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;
using Greta.Sdk.Core.Abstractions;

namespace Greta.BO.BusinessLogic.Models.Dto;

public class TimeKeepingWorkTimeModel: IDtoLong<string>, IMapFrom<TimeKeeping>
{
    public DateTime? Begin { get; set; }   
    public string BeginFormatDate { get; set; }   
    public DateTime? End { get; set; }  
    public string EndFormatDate { get; set; }   
    public long EmployeeId { get; set; }
    public string EmployeeName { get; set; }
    public long? EndDeviceId { get; set; }
    public long? BeginDeviceId { get; set; }

    public double TimeWorked { get; set; }
    public string TimeWorkedFormat { get; set; }


    public long Id { get; set; }
    public bool State { get; set; }
    public string UserCreatorId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int Week { get; set; }
}