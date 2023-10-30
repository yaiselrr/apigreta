using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;
using Greta.BO.BusinessLogic.Models.Dto.Search;

namespace Greta.BO.Api.Dto.Search
{
    public class CategorySearchDto : BaseSearchDto, IMapFrom<CategorySearchModel>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public long DepartmentId { get; set; }
    }
}