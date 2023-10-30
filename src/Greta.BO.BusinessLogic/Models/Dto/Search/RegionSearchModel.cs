using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;

namespace Greta.BO.BusinessLogic.Models.Dto.Search
{
    public class RegionSearchModel : BaseSearchModel, IMapFrom<Region>
    {
        public string Name { get; set; }
    }
}