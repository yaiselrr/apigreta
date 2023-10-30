using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;
using Greta.BO.BusinessLogic.Models.Dto.Search;

namespace Greta.BO.Api.Dto.Search
{
    public class VendorProductSearchDto : BaseSearchDto, IMapFrom<VendorProductSearchModel>
    {
        public long VendorId { get; set; }
        public long ProductId { get; set; }
    }
}