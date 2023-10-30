namespace Greta.BO.BusinessLogic.Models.Dto
{
    public class ProcessEndOfDayResponse
    {
        public long ElementId { get; set; }
        public string ElementName { get; set; }
        public string StoreName { get; set; }
        public long StoreId { get; set; }
        
        public string InitDate { get; set; }
        public string EndDate { get; set; }

        public decimal ManualDiscount { get; set; }
        public decimal UpcReturns { get; set; }
        
        public decimal TotalCheck { get; set; }
        

        public int StartingCash { get; set; }
        public decimal CashTotalCounted { get; set; }
        public decimal CashToDeposit { get; set; }
        public decimal TenderedCashTotal { get; set; }
        public decimal CashOverShort { get; set; }
        
        public decimal TotalNotTaxableSales { get; set; }
        public decimal TotalTaxableSales { get; set; }
        public decimal SalesTaxCollected { get; set; }
        public decimal TotalSales { get; set; }


        public decimal TotalFeeAndCharges { get; set; }

        public decimal BottleReturnTotal { get; set; }
        public decimal RefundReturn { get; set; }
        public decimal RefundReturnEbt { get; set; }
        public decimal RefundReturnOther { get; set; }
        public decimal PaidOut { get; set; }
        public decimal TotalCashOut { get; set; }

        public decimal DebitCashBack { get; set; }
        public decimal EBTCashBack { get; set; }
        public decimal TotalCash { get; set; }

        public decimal CreditCardSales { get; set; }
        public decimal SnapEBTSales { get; set; }
        public decimal GiftCardSales { get; set; }

    }
}