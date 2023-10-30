using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;
using Greta.BO.BusinessLogic.Models.Dto.Search;

namespace Greta.BO.Api.Dto.Search
{
    public class ScaleCategorySearchDto : BaseSearchDto, IMapFrom<ScaleCategorySearchModel>
    {
        public string Name { get; set; }
        public long DepartmentId { get; set; }
    }
}