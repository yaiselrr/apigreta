using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;

namespace Greta.BO.BusinessLogic.Models.Dto.Search
{
    public class StoreSearchModel : BaseSearchModel, IMapFrom<Store>
    {
        public string Name { get; set; }
        public long RegionId { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}