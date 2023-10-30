using Greta.BO.Api.Entities;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;

namespace Greta.BO.BusinessLogic.Models.Dto.Search
{
    public class ProductSearchModel : BaseSearchModel, IMapFrom<Product>
    {
        public ProductType ProductType { get; set; }
        public int ProductTypeExcept { get; set; } = -1;

        public string UPC { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public bool State { get; set; }

        public long DepartmentId { get; set; }
        public long CategoryId { get; set; }
        public long FamilyId { get; set; }
    }
}