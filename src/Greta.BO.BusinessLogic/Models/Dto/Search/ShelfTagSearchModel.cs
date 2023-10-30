using Greta.BO.Api.Entities;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;

namespace Greta.BO.BusinessLogic.Models.Dto.Search
{
    public class ShelfTagSearchModel : BaseSearchModel, IMapFrom<ShelfTag>
    {
        public string UPC { get; set; }
        public string ProductName { get; set; }
        public long ProductId { get; set; }

        public long DepartmentId { get; set; }
        public long CategoryId { get; set; }
        public long BinLocationId { get; set; }
        
        public bool State { get; set; }
    }
}