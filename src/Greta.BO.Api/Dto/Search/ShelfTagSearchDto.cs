using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;
using Greta.BO.BusinessLogic.Models.Dto.Search;

namespace Greta.BO.Api.Dto.Search
{
    public class ShelfTagSearchDto : BaseSearchDto, IMapFrom<ShelfTagSearchModel>
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