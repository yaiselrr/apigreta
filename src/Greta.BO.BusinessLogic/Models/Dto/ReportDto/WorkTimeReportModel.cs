using Greta.BO.Api.Entities.Attributes;
using Greta.Sdk.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Greta.BO.BusinessLogic.Models.Dto.ReportDto
{
    public class WorkTimeReportModel
    {
        public static IEnumerable<(PropertyInfo property, FieldInfoAttribute field)> GetFieldInfo<T>()
           => typeof(T)
               .GetProperties()               
               .Select(prop => (prop, prop.GetCustomAttribute<FieldInfoAttribute>()));

        public long EmployeeId { get; set; }
        public string EmployeeName { get; set; }

        // Hour worked value by day
        public double WtSundayValue { get; set; }
        public double WtMondayValue { get; set; }
        public double WtTuesdayValue { get; set; }
        public double WtWednesdayValue { get; set; }
        public double WtThursdayValue { get; set; }
        public double WtFridayValue { get; set; }
        public double WtSaturdayValue { get; set; }       
        public double WtWeekValue { get; set; }

        // String worked by day
        public string WtSunday { get; set; }
        public string WtMonday { get; set; }
        public string WtTuesday { get; set; }
        public string WtWednesday { get; set; }
        public string WtThursday { get; set; }
        public string WtFriday { get; set; }
        public string WtSaturday { get; set; }
        
        public string WtWeek { get; set; }

        // Start and end of week
        public DateTime WeekStart { get; set; }
        public DateTime WeekEnd { get; set; }
        public int NumberWeek { get; set; }
    }
}
