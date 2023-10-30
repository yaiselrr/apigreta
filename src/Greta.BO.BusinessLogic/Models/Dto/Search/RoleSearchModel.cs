using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;

namespace Greta.BO.BusinessLogic.Models.Dto.Search
{
    public class RoleSearchModel : BaseSearchModel, IMapFrom<Role>
    {
        public string Name { get; set; }
    }
}