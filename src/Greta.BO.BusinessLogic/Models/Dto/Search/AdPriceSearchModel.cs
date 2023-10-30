using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;

namespace Greta.BO.BusinessLogic.Models.Dto.Search
{
    public class AdBatchSearchModel : BaseSearchModel, IMapFrom<AdBatch>
    {
        public string Name { get; set; }

        // public DateTime? StartTime { get; set; }
        public long StoreId { get; set; }

        // public DateTime? EndTime { get; set; }
    }
}