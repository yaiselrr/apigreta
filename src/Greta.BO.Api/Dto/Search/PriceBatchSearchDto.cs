using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;
using Greta.BO.BusinessLogic.Models.Dto.Search;

namespace Greta.BO.Api.Dto.Search
{
    public class PriceBatchSearchDto : BaseSearchDto, IMapFrom<PriceBatchSearchModel>
    {
        public string Name { get; set; }

        // public DateTime? StartTime { get; set; }
        public long StoreId { get; set; }
    }
}