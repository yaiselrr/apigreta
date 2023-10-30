using System;

namespace Greta.BO.Api.Entities;

public class TimeKeeping: BaseEntityLong
{
    public DateTime? Begin { get; set; }   
    public string BeginFormatDate { get; set; }   
    public DateTime? End { get; set; }  
    public string EndFormatDate { get; set; }   
    public long EmployeeId { get; set; }
    public string EmployeeName { get; set; }
    public long? EndDeviceId { get; set; }
    public long? BeginDeviceId { get; set; }
    
    public long? BeginStoreId { get; set; }
    public string BeginStoreName { get; set; }
    
    public long? EndStoreId { get; set; }
    public string EndStoreName { get; set; }
    public double TimeWorked { get; set; }
    public string TimeWorkedFormat { get; set; }

    public long? UserForceBeginId { get; set; }
    public string UserForceBegin { get; set; }
    public long? UserForceEndId { get; set; }
    public string UserForceEnd { get; set; }
}