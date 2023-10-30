using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;
using Greta.BO.BusinessLogic.Models.Dto.Search;

namespace Greta.BO.Api.Dto.Search
{
    public class StoreProductSearchDto : BaseSearchDto, IMapFrom<StoreProductSearchModel>
    {
        public long StoreId { get; set; }
    }
}