using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;

namespace Greta.BO.BusinessLogic.Models.Dto.Search
{
    public class ScalendarSearchModel : BaseSearchModel, IMapFrom<Scalendar>
    {
        public string Day { get; set; }
    }
}