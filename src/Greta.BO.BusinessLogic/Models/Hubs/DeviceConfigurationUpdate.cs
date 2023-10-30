

using System;
using Greta.BO.Api.Entities.Enum;

namespace Greta.BO.BusinessLogic.Models.Hubs
{
    public class DeviceConfigurationUpdate
    {
        public bool ServerData { get; set; }
        public string DeviceName { get; set; }
        public long DeviceNumber { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Slogan { get; set; }
        public string Zip { get; set; }
        public PrinterType PrinterType { get; set; }

        /// <summary>
        ///     If type is Remote then ip:port
        ///     if type is local then printer name
        /// </summary>
        /// <value>ex: 192.168.0.1:9001 or Epson4625</value>
        public string PrinterName { get; set; }

        public PrinterType LabelPrinterType { get; set; }
        

        /// <summary>
        ///     If type is Remote then ip:port
        ///     if type is local then printer name
        /// </summary>
        /// <value>ex: 192.168.0.1:9001 or Epson4625</value>
        public string LabelPrinterName { get; set; }

        public ScaleModel ScaleModel { get; set; }


        /// <summary>
        ///     COM1, COM2 example
        /// </summary>
        /// <value></value>
        public string ScaleComName { get; set; }

        /// <summary>
        ///     Scale BaudRate
        /// </summary>
        /// <value></value>
        public int ScaleBaudRate { get; set; }

        public GiftCardType GiftCardType { get; set; }
        public PaymentGatewayType PaymentGatewayType { get; set; }
        public string PaymentGatewayPosLinkDestPort { get; set; }
        public string PaymentGatewayPosLinkCommType { get; set; }
        public string PaymentGatewayPosLinkSerialPort { get; set; }
        public string PaymentGatewayPosLinkBaudRate { get; set; }
        public string PaymentGatewayPosLinkDestIP { get; set; }
        public string PaymentGatewayPosLinkTimeOut { get; set; }
        
        public string ElasticClient { get; set; }
        public string ElasticStore { get; set; }
        public string ElasticDevice { get; set; }
        public string ElasticUrl { get; set; }

        public string ElasticUserName { get; set; }

        public string ElasticPassword { get; set; }

        public int ElasticLogLevel { get; set; }
        
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
        public POSTheme Theme { get; set; }
        

        #endregion
        
        
        public bool PrintStoreNameOnReceipt { get; set; }
    }
}
