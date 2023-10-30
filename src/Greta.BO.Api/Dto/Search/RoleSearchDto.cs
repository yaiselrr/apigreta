using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;
using Greta.BO.BusinessLogic.Models.Dto.Search;

namespace Greta.BO.Api.Dto.Search
{
    public class RoleSearchDto : BaseSearchDto, IMapFrom<RoleSearchModel>
    {
        public string Name { get; set; }
    }
}