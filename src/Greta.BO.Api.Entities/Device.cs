using System;
using System.Collections.Generic;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.Api.Entities.Interfaces;

namespace Greta.BO.Api.Entities
{
    public class Device : BaseEntityLong, INameUniqueEntity
    {
        public string Name { get; set; }
        public string DeviceId { get; set; }
        public Guid GuidId { get; set; }
        public int SynchroVersion { get; set; }
        public string SignalRConnectionId { get; set; }

        public bool BrokerAlive { get; set; }
        public bool WorkerAlive { get; set; }
        public string BrokerVersion { get; set; }
        public string WorkerVersion { get; set; }
        public string RealTimeRaw { get; set; }

        /// <summary>
        ///     Used for disconnect not reported elements
        /// </summary>
        /// <value></value>
        public DateTime LastPongTime { get; set; }

        public long StoreId { get; set; }
        public Store Store { get; set; }

        public virtual List<BatchClose> BatchClose { get; set; }

        #region Configuration

        #region Printers and Scales

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

        /// <summary>
        ///     Printers names coma separate
        /// </summary>
        public string Printers { get; set; }

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

        #endregion

        #region Paymentgateway
        #region Poslink
        
        public PaymentGatewayType PaymentGatewayType { get; set; }
        public string PaymentGatewayPosLinkDestPort { get; set; }
        public string PaymentGatewayPosLinkCommType { get; set; }
        public string PaymentGatewayPosLinkSerialPort { get; set; }
        public string PaymentGatewayPosLinkBaudRate { get; set; }
        /// <summary>
        /// Used for equinox too
        /// </summary>
        public string PaymentGatewayPosLinkDestIP { get; set; }
        public string PaymentGatewayPosLinkTimeOut { get; set; }
        
        #endregion
        #endregion
        
        #endregion

        public virtual List<GiftCard> GiftCards { get; set; }
        public virtual List<ExternalScale> ExternalScales { get; set; }
    }
}