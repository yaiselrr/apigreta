using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;
using Greta.BO.BusinessLogic.Models.Dto.Search;

namespace Greta.BO.Api.Dto.Search
{
    public class OnlineStoreSearchDto : BaseSearchDto, IMapFrom<OnlineStoreSearchModel>
    {
        public string Name { get; set; }
        public long StoredId { get; set; }
    }
}