 
namespace Greta.BO.Api.Entities
{
    public class SaleTender : BaseEntityLong
    {
        public long SaleId { get; set; }
        public Sale Sale { get; set; }

        public string Name { get; set; }
        public decimal Amount { get; set; }
        public string RawResponse { get; set; }

        #region PaymentGateway
        public string ResultCode { get;  set; }
        public string ResultTxt { get;  set; }
        public string RefNum { get;  set; }
        public decimal RequestedAmount { get;  set; }
        public decimal ApprovedAmount { get;  set; }
        public decimal RemainingBalance { get;  set; }
        public decimal ExtraBalance { get;  set; }
        public string BogusAccountNum { get;  set; }
        public string CardType { get;  set; }
        public string AuthCode { get;  set; }
        public decimal CashBack { get;  set; }
        /// <summary>
        /// name on card
        /// </summary>
        public string CardHolderName { get; set; }
        /// <summary>
        /// Hold Card operator Amex, visa etc
        /// </summary>
        public string IssuerName { get; set; }
        public string Pan { get; set; }
        
        public string HostCode { get; set; }
        public string HostDetailedMessage { get; set; }
        public string HostResponse { get; set; }

        # endregion
    }
}
