using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;

namespace Greta.BO.BusinessLogic.Models.Dto.Search
{
    public class VendorProductSearchModel : BaseSearchModel, IMapFrom<VendorProduct>
    {
        public long VendorId { get; set; }
        public long ProductId { get; set; }
    }
}