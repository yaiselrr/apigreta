using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;

namespace Greta.BO.BusinessLogic.Models.Dto.Search
{
    public class CategorySearchModel : BaseSearchModel, IMapFrom<Category>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public long DepartmentId { get; set; }
    }
}