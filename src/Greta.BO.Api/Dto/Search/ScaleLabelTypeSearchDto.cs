using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;
using Greta.BO.BusinessLogic.Models.Dto.Search;

namespace Greta.BO.Api.Dto.Search
{
    public class ScaleLabelTypeSearchDto : BaseSearchDto, IMapFrom<ScaleLabelTypeSearchModel>
    {
        public string Name { get; set; }

        // public ScaleType ScaleType { get; set; }
    }
}