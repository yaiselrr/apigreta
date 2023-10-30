using System;
using System.Collections.Generic;
using Greta.BO.Api.Entities.Enum;
using Greta.Sdk.EFCore.Interfaces;
using Greta.BO.Api.Entities.Interfaces;

namespace Greta.BO.Api.Entities
{
    public class Store : BaseEntityLong, INameUniqueEntity, ISoftDelete
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Slogan { get; set; }
        public string Zip { get; set; }
        public long RegionId { get; set; }
        public virtual Region Region { get; set; }
        /// <summary>
        ///     Determine if this store is fully synced.
        /// </summary>
        public bool Updated { get; set; }

        public DateTime LastBackupTime { get; set; }
        public string LastBackupPath { get; set; }

        public int LastBackupVersion { get; set; }

        public decimal CreditCardCalculation { get; set; }

        public int SynchroVersion { get; set; }

        /// <summary>
        ///     List of string coma separate
        /// </summary>
        public string RemotePrinters { get; set; }



        #region Configuration
        public GiftCardType GiftCardType { get; set; }
        public DrawerTraking DrawerTraking { get; set; }
        public string Language { get; set; }
        public string Currency { get; set; }

        public bool CashDiscount { get; set; }
        public decimal CashDiscountValue { get; set; }
        public bool ClientTransparency { get; set; }

        /// <summary>
        /// Checks      add check box with “Accept Check for the amount of purchase only”
        /// </summary>
        public bool AcceptChecksExactAmount { get; set; }

        /// <summary>
        /// Credit Card  Check box with a box for dollar entry text box. “Signature required for Sales over $ 0.00 amount”
        /// </summary>
        public bool CreditCardNeedSignature { get; set; }
        public decimal CreditCardNeedSignatureAmount { get; set; }
        /// <summary>
        /// Debit Card  Check box “Allow Cash Back?” text box “Maximum Cash Back $ 0.00 amount”
        /// </summary>
        public bool DebitCardCashBack { get; set; }
        public decimal DebitCardCashBackMaxAmount { get; set; }

        /// <summary>
        /// Snap/EBT Cash    Check box “Allow cashback” Text box Maximum amount of cashback $0.00”  We will always print the Snap/EBT cash balance on the receipt after a sale.
        /// </summary>
        public bool SnapEBTCAshCashBack { get; set; }
        public decimal SnapEBTCAshCashBackMaxAmount { get; set; }

        public bool MinimumAgeRequired { get; set; }
        public bool DisplayChangeDueAfterTender { get; set; }
        public bool DisplayLaneClosed { get; set; }
        public bool UseCustomer { get; set; }
        
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
        public bool PrintStoreNameOnReceipt { get; set; }
        
        public string TaxPayerId { get; set; }
        
        public string LiquorLicenseId { get; set; }

        #region Traceability

        public string UsdaEstablishNumber { get; set; }
        public string GlobalTraceNumberGtn { get; set; }
        #endregion
        #endregion


        //public virtual List<StoreProduct> StoreProducts { get; set; }

        public virtual List<ExternalScale> ExternalScales { get; set; }
        public virtual List<Device> Devices { get; set; }
        public virtual List<Tax> Taxs { get; set; }

        public virtual List<Synchro> Synchros { get; set; }
        public virtual List<BOUser> Employees { get; set; }
        public virtual List<Batch> Batchs { get; set; }

        public virtual List<GiftCard> GiftCards { get; set; }
      
        
        public DateTime OpenTime { get; set; }
        public DateTime ClosedTime { get; set; }
        public POSTheme Theme { get; set; }
        
        public string TimeZoneId { get; set; }

        public Guid GuidId { get; set; }
        
        public LoyaltyDiscount LoyaltyDiscount { get; set; }
        
        public virtual List<OnlineStore> OnlineStores { get; set; }
        public virtual List<VendorOrder> VendorOrders { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}