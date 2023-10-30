using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;
using Greta.BO.BusinessLogic.Models.Dto.Search;

namespace Greta.BO.Api.Dto.Search
{
    public class ScalendarSearchDto : BaseSearchDto, IMapFrom<ScalendarSearchModel>
    {
        public string Day { get; set; }
    }
}