using Greta.BO.Api.Entities.Enum;

namespace Greta.BO.Api.Dto.Search
{
    public class SynchroSearchDto : BaseSearchDto
    {
        public SynchroStatus Status { get; set; }
    }
}