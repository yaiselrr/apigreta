using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;
using Greta.BO.BusinessLogic.Models.Dto.Search;

namespace Greta.BO.Api.Dto.Search
{
    public class PriceBatchDetailSearchDto : BaseSearchDto, IMapFrom<PriceBatchDetailSearchModel>
    {
        public long ProductId { get; set; }
        public long HeaderId { get; set; }
    }
}