using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;
using Greta.BO.BusinessLogic.Models.Enums;
using System;

namespace Greta.BO.BusinessLogic.Models.Dto.Search
{
    public class WorkTimeSearchModel : BaseSearchModel
    {
        public TimeKeepingUserFilterMode Mode { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }
}