using System;
using Greta.BO.BusinessLogic.Models.Enums;

namespace Greta.BO.BusinessLogic.Models.Dto.Search;

public class TimeKeepingUserSearchModel: BaseSearchModel
{
    public long EmployeeId { get; set; }
    public TimeKeepingUserFilterMode Mode { get; set; }
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
}