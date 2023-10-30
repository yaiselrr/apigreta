using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;

namespace Greta.BO.BusinessLogic.Models.Dto.Search
{
    public class DepartmentSearchModel : BaseSearchModel, IMapFrom<Department>
    {
        public string Name { get; set; }
        public int DepartmentId { get; set; }
    }
}