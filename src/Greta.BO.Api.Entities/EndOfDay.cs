using System;
using System.Collections.Generic;
using Greta.BO.Api.Entities.Enum;

namespace Greta.BO.Api.Entities
{
    public class EndOfDay: BaseEntityLong
    {
        //the same of store in the momment of endofday
        public DateTime? SaleDay { get; set; }
        public DrawerTraking TrackingType { get; set; }
        public long StoreId { get; set; }
        public long ElementId { get; set; }
        public string ElementName { get; set; }

        public int SalesCount { get; set; }

        #region Resume
        public decimal CashTotalCounted { get; set; }
        public decimal CashToDeposit { get; set; }
        public decimal TenderedCashTotal { get; set; }
        public decimal CashOverShort { get; set; }
        #endregion
        
        #region OnlyReport

        public decimal TotalNotTaxableSales { get; set; }
        public decimal SalesTaxCollected { get; set; }
        public decimal TotalTaxableSales { get; set; }
        public decimal TotalSales { get; set; }

        public List<SaleTaxResume> Taxes { get; set; }

        public decimal TotalFeeAndCharges { get; set; }

        public decimal BottleReturnTotal { get; set; }
        public decimal RefundReturn { get; set; }
        public decimal RefundReturnOther { get; set; }
        public decimal RefundReturnEbt { get; set; }
        public decimal PaidOut { get; set; }

        public decimal ManualDiscount { get; set; }
        public decimal UpcReturns { get; set; }
        public decimal TotalCheck { get; set; }
        
        
        //cAshBAck
        public decimal DebitCashBack { get; set; }
        public decimal EBTCashBack { get; set; }

        /// <summary>
        /// ( Tendered Cash - (Bottle Refund + Return if Cash + Cash Back on Debitand EBT))
        /// </summary>
        public decimal TotalCash { get; set; }

        public decimal CreditCardSales { get; set; }
        public decimal SnapEBTSales { get; set; }
        public decimal GiftCardSales { get; set; }
        #endregion

        #region Counting
        public int StartingCash { get; set; }
        public int Count100 { get; set; }
        public int Count50 { get; set; }
        public int Count20 { get; set; }
        public int Count10 { get; set; }
        public int Count5 { get; set; }
        public int Count1 { get; set; }
        public int Countc100 { get; set; }
        public int Countc50 { get; set; }
        public int Countc25 { get; set; }
        public int Countc10 { get; set; }
        public int Countc5 { get; set; }
        public int Countc1 { get; set; }
        #endregion

        public List<Sale> Sales { get; set; }
    }

    public class SaleTaxResume: BaseEntityLong
    {
        public string Name { get; set; }
        public long TaxId { get; set; }
        public decimal Amount { get; set; }

        public long EndOfDayId { get; set; }
        public EndOfDay EndOfDay { get; set; }
    }
}