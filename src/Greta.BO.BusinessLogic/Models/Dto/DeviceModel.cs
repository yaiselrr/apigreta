using System;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;
using Greta.Sdk.Core.Abstractions;

namespace Greta.BO.BusinessLogic.Models.Dto
{
    public class DeviceModel : IDtoLong<string>, IMapFrom<Device>
    {
        public string Name { get; set; }

        public string DeviceId { get; set; }

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

        public long Id { get; set; }
        public bool State { get; set; }
        public string UserCreatorId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

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
        public string PaymentGatewayPosLinkDestIP { get; set; }
        public string PaymentGatewayPosLinkTimeOut { get; set; }

        #endregion
        #endregion

        #endregion
    }
}