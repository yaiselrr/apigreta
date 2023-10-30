using Greta.BO.Api.Entities.Enum;

namespace Greta.BO.BusinessLogic.Models.Dto.Search
{
    public class SynchroSearchModel : BaseSearchModel
    {
        public long RegionId { get; set; }

        public SynchroStatus Status { get; set; }
    }
}