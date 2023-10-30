using Greta.BO.Api.Entities.Enum;
using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;

namespace Greta.BO.BusinessLogic.Models.Dto.Search
{
    public class ReportSearchModel : BaseSearchModel, IMapFrom<Api.Entities.Report>
    {
        public string Name { get; set; }
        public ReportCategory Category { get; set; }
    }
}
