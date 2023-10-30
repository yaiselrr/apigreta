using System;
using Greta.Sdk.MassTransit.Interfaces;

namespace Greta.BO.Api.EventContracts.Store
{
    public interface PutStoreConfigurationRequestContract: IRegisteredEventContract
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Slogan { get; set; }
        public string Zip { get; set; }

        public long Id { get; set; }

        public decimal CreditCardCalculation { get; set; }

        #region Configuration
        public int GiftCardType { get; set; }
        public int DrawerTraking { get; set; }
        public string Language { get; set; }
        public string Currency { get; set; }

        public bool CashDiscount { get; set; }
        public decimal CashDiscountValue { get; set; }
        public bool ClientTransparency { get; set; }

        public bool AcceptChecksExactAmount { get; set; }
        
        public bool CreditCardNeedSignature { get; set; }
        public decimal CreditCardNeedSignatureAmount { get; set; }
        
        public bool DebitCardCashBack { get; set; }
        public decimal DebitCardCashBackMaxAmount { get; set; }
        
        public bool SnapEBTCAshCashBack { get; set; }
        public decimal SnapEBTCAshCashBackMaxAmount { get; set; }
        
        public bool MinimumAgeRequired { get; set; }
        public bool DisplayChangeDueAfterTender { get; set; }
        public bool DisplayLaneClosed { get; set; }
        public bool UseCustomer { get; set; }
        
        public bool PrintStoreNameOnReceipt { get; set; }
        
        public bool UseTaxOverride { get; set; }
        public bool UseNoSale { get; set; }
        public bool UsePaidOut { get; set; }
        public bool UseReturn { get; set; }
        public bool UseDiscount { get; set; }
        public bool UseZeroScale { get; set; }
        public bool UseBottleRefund { get; set; }
        public bool UseGiftCards { get; set; }
        public bool UseExchange { get; set; }
        
        public bool UseReprintReceipt { get; set; }
        public bool UseEBTCheckBalance { get; set; }
        public bool UseRemoveserviceFee { get; set; }
        
        
        public decimal DefaulBottleDeposit { get; set; }
        public bool PrintReceiptOptional { get; set; }
        public int AutoLogOffCachiers { get; set; }
        public DateTime AutoEndDate { get; set; }
        public bool AutoCloseAllCachiers { get; set; }
        
        public DateTime OpenTime { get; set; }
        public DateTime ClosedTime { get; set; }
        public int Theme { get; set; }
        public string TimeZoneId { get; set; }
        public string UsdaEstablishNumber { get; set; }
        public string GlobalTraceNumberGtn { get; set; }
        
        public string TaxPayerId { get; set; }
        public string LiquorLicenseId { get; set; }
        #endregion
    }
}