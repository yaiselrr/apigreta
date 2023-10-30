using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;
using Greta.BO.BusinessLogic.Models.Dto.Search;

namespace Greta.BO.Api.Dto.Search
{
    public class AdBatchSearchDto : BaseSearchDto, IMapFrom<AdBatchSearchModel>
    {
        public string Name { get; set; }

        // public DateTime? StartTime { get; set; }
        public long StoreId { get; set; }

        // public DateTime? EndTime { get; set; }
    }
}