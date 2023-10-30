using Greta.BO.Api.Entities;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;

namespace Greta.BO.BusinessLogic.Models.Dto.Search
{
    public class ScaleLabelTypeSearchModel : BaseSearchModel, IMapFrom<ScaleLabelType>
    {
        public string Name { get; set; }
        public LabelDesignMode Type { get; set; }
    }
}