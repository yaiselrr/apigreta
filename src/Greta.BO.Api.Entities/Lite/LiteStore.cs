using System;
using Greta.BO.Api.Entities.Enum;

namespace Greta.BO.Api.Entities.Lite
{
    public class LiteStore : BaseEntityLong
    {
        public string StoreCode { get; set; }
        public string StoreName { get; set; }

        public int SynchroVersion { get; set; }

        #region Configuration
        public DrawerTraking DrawerTraking { get; set; }
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
        public decimal DefaulBottleDeposit { get; set; }
        public bool PrintReceiptOptional { get; set; }
        public int AutoLogOffCachiers { get; set; }
        public DateTime AutoEndDate { get; set; }
        public bool AutoCloseAllCachiers { get; set; }

        #endregion

        public static LiteStore Convert(Store from)
        {
            return new LiteStore
            {
                Id = from.Id,
                StoreCode = from.Id.ToString(),
                State = from.State,
                UserCreatorId = from.UserCreatorId,
                CreatedAt = from.CreatedAt,
                UpdatedAt = from.UpdatedAt,
                StoreName = from.Name,
                SynchroVersion = from.SynchroVersion,
                Language = from.Language,
                Currency = from.Currency,
                CashDiscount = from.CashDiscount,
                CashDiscountValue = from.CashDiscountValue,
                ClientTransparency = from.ClientTransparency,
                AcceptChecksExactAmount = from.AcceptChecksExactAmount,
                CreditCardNeedSignature = from.CreditCardNeedSignature,
                CreditCardNeedSignatureAmount = from.CreditCardNeedSignatureAmount,
                DebitCardCashBack = from.DebitCardCashBack,
                DebitCardCashBackMaxAmount = from.DebitCardCashBackMaxAmount,
                SnapEBTCAshCashBack = from.SnapEBTCAshCashBack,
                SnapEBTCAshCashBackMaxAmount = from.SnapEBTCAshCashBackMaxAmount,
                MinimumAgeRequired = from.MinimumAgeRequired,
                DisplayChangeDueAfterTender = from.DisplayChangeDueAfterTender,
                DisplayLaneClosed = from.DisplayLaneClosed,
                DefaulBottleDeposit = from.DefaulBottleDeposit,
                PrintReceiptOptional = from.PrintReceiptOptional,
                AutoLogOffCachiers = from.AutoLogOffCachiers,
                AutoEndDate = from.AutoEndDate,
                AutoCloseAllCachiers = from.AutoCloseAllCachiers,
                DrawerTraking = from.DrawerTraking
            };
        }
    }
}