using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;

namespace Greta.BO.BusinessLogic.Models.Dto.Search
{
    public class PriceBatchSearchModel : BaseSearchModel, IMapFrom<PriceBatch>
    {
        public string Name { get; set; }

        // public DateTime? StartTime { get; set; }
        public long StoreId { get; set; }
    }
}