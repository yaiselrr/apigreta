using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;

namespace Greta.BO.BusinessLogic.Models.Dto.Search
{
    public class PriceBatchDetailSearchModel : BaseSearchModel, IMapFrom<PriceBatchDetail>
    {
        public long ProductId { get; set; }
        public long HeaderId { get; set; }
    }
}