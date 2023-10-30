using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;

namespace Greta.BO.BusinessLogic.Models.Dto.Search
{
    public class CutListTemplateSearchModel : BaseSearchModel, IMapFrom<CutListTemplate>
    {
        public string Name { get; set; }
        public long CutListTemplateId { get; set; }
    }
}