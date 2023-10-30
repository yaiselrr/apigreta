using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;

namespace Greta.BO.BusinessLogic.Models.Dto.Search
{
    public class RoundingTableSearchModel : BaseSearchModel, IMapFrom<RoundingTable>
    {
        public int EndWith { get; set; }
    }
}