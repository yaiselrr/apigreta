using System;

namespace Greta.BO.Api.Entities
{
    public class ChangePriceReasonCode: BaseEntityLong
    {
        public long ReasonCodeId { get; set; }
        public string ReasonCodeName { get; set; }

        public long EmployeeId { get; set; }
        public long DeviceId { get; set; }
        public string EmployeeName { get; set; }
        public string DeviceName { get; set; }
        public decimal OldPrice { get; set; }
        public decimal NewPrice { get; set; }
        public DateTime ChangeTime { get; set; }
    }
}
