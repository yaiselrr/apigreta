

using Greta.BO.Api.Entities.Enum;

namespace Greta.BO.BusinessLogic.Models.Hubs
{
    public class GiftCardSaleResponse
    {
        public decimal ApprovedAmount { get; set; }
        public decimal RemainAmount { get; set; }
        public long GiftCardTransactionId { get; set; }
        public GiftCardRequestStatus Status { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
    }
}
