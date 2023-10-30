using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;

namespace Greta.BO.BusinessLogic.Models.Dto.Search
{
    public class StoreProductSearchModel : BaseSearchModel, IMapFrom<StoreProduct>
    {
        public long StoreId { get; set; }
    }
}