using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;

namespace Greta.BO.BusinessLogic.Models.Dto.Search
{
    public class ScaleCategorySearchModel : BaseSearchModel, IMapFrom<ScaleCategory>
    {
        public string Name { get; set; }
        public long DepartmentId { get; set; }
    }
}