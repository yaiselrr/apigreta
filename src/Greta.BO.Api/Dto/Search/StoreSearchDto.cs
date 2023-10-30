using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;
using Greta.BO.BusinessLogic.Models.Dto.Search;

namespace Greta.BO.Api.Dto.Search
{
    public class StoreSearchDto : BaseSearchDto, IMapFrom<StoreSearchModel>
    {
        public string Name { get; set; }
        public long RegionId { get; set; }
    }
}