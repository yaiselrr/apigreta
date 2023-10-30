using Greta.BO.Api.Entities.Enum;

namespace Greta.BO.BusinessLogic.Models.Dto.Search
{
    public class VendorOrderSearchModel: BaseSearchModel
    {
        public long VendorId { get; set; }
        
        public long StoreId { get; set; }
        public string VendorName { get; set; }
        public string  StoreName { get; set; }
        public string InvoiceNumber { get; set; }
        public VendorOrderStatus? Status { get; set; }

        public bool IsDsd { get; set; }
    }
}