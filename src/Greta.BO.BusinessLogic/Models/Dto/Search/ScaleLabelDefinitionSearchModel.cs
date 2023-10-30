using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;

namespace Greta.BO.BusinessLogic.Models.Dto.Search
{
    public class ScaleLabelDefinitionSearchModel : BaseSearchModel, IMapFrom<ScaleLabelDefinition>
    {
        public long ScaleProductId { get; set; }
        public long ScaleLabelType1Id { get; set; }
        public long ScaleLabelType2Id { get; set; }
        public long ScaleBrandId { get; set; }
    }
}