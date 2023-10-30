using Greta.BO.Api.Entities.Enum;
using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;
using Greta.BO.BusinessLogic.Models.Dto.Search;

namespace Greta.BO.Api.Dto.Search
{
    public class ReportSearchDto : BaseSearchDto, IMapFrom<ReportSearchModel>
    {
        public string Name { get; set; }
        public ReportCategory Category { get; set; }
    }
}
