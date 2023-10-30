using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;

namespace Greta.BO.BusinessLogic.Models.Dto.Search
{
    public class OnlineStoreSearchModel : BaseSearchModel, IMapFrom<OnlineStore>
    {
        public string Name { get; set; }
        public long StoredId { get; set; }
    }
}