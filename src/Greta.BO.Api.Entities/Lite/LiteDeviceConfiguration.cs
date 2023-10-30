using Greta.BO.Api.Entities.Enum;

namespace Greta.BO.Api.Entities.Lite
{
    public class LiteDeviceConfiguration : BaseEntityLong
    {
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


        public static LiteDeviceConfiguration Convert(Device from)
        {
            return new LiteDeviceConfiguration
            {
                Id = from.Id,
                State = from.State,
                UserCreatorId = from.UserCreatorId,
                CreatedAt = from.CreatedAt,
                UpdatedAt = from.UpdatedAt,
                PrinterType = from.PrinterType,
                PrinterName = from.PrinterName,
                LabelPrinterType = from.LabelPrinterType,
                LabelPrinterName = from.LabelPrinterName,
                Printers = from.Printers,
                ScaleModel = from.ScaleModel,
                ScaleComName = from.ScaleComName,
                ScaleBaudRate = from.ScaleBaudRate,

                PaymentGatewayType = from.PaymentGatewayType,
                PaymentGatewayPosLinkCommType = from.PaymentGatewayPosLinkCommType,
                PaymentGatewayPosLinkSerialPort = from.PaymentGatewayPosLinkSerialPort,
                PaymentGatewayPosLinkBaudRate = from.PaymentGatewayPosLinkBaudRate,
                PaymentGatewayPosLinkDestIP = from.PaymentGatewayPosLinkDestIP,
                PaymentGatewayPosLinkDestPort = from.PaymentGatewayPosLinkDestPort,
                PaymentGatewayPosLinkTimeOut = from.PaymentGatewayPosLinkTimeOut,
            };
        }
    }
}