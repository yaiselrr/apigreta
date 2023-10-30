using System.Threading;
using System.Threading.Tasks;
using Greta.BO.BusinessLogic.Hubs;
using Greta.BO.BusinessLogic.Models.Hubs;
using Greta.BO.BusinessLogic.Service;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Command.DeviceConfiguration;

public record SendToDeviceCommand(long? StoreId, long? DeviceId) : IRequest;

public class SendToDeviceHandler : IRequestHandler<SendToDeviceCommand>
{
    private readonly ILogger<SendToDeviceHandler> _logger;
    private readonly IDeviceService _deviceService;
    private readonly IStoreService _storeService;
    private readonly IConfiguration _configuration;
    private readonly IHubContext<CloudHub, ICloudHub> _hub;

    public SendToDeviceHandler(
        ILogger<SendToDeviceHandler> logger,
        IDeviceService deviceService,
        IStoreService storeService,
        IConfiguration configuration,
        IHubContext<CloudHub, ICloudHub> hub
    )
    {
        _logger = logger;
        _deviceService = deviceService;
        _storeService = storeService;
        _configuration = configuration;
        _hub = hub;
    }

    public async Task Handle(SendToDeviceCommand request, CancellationToken cancellationToken)
    {
        Api.Entities.Store store = null;
        Api.Entities.Device device = null;
        if (request.StoreId != null)
        {
            store = await _storeService.Get(request.StoreId.Value);
        }

        if (request.DeviceId != null)
        {
            device = await _deviceService.Get(request.DeviceId.Value);

            if (device != null)
            {
                if (device.SignalRConnectionId == null)
                {
                    _logger.LogError("Device not connected");
                    return;
                }

                store = await _storeService.Get(device.StoreId);
            }
        }

        if (store == null)
        {
            // dont send any data if store is not present
            _logger.LogError("Store not detected");
            return;
        }

        var newConfig = new DeviceConfigurationUpdate()
        {
            ServerData = true,
            Name = store.Name,
            Phone = store.Phone,
            Address = store.Address,
            Slogan = store.Slogan,
            Zip = store.Zip,

            DrawerTraking = store.DrawerTraking,
            Language = store.Language,
            Currency = store.Currency,
            CashDiscount = store.CashDiscount,
            CashDiscountValue = store.CashDiscountValue,
            ClientTransparency = store.ClientTransparency,
            AcceptChecksExactAmount = store.AcceptChecksExactAmount,
            CreditCardNeedSignature = store.CreditCardNeedSignature,
            CreditCardNeedSignatureAmount = store.CreditCardNeedSignatureAmount,
            DebitCardCashBack = store.DebitCardCashBack,
            DebitCardCashBackMaxAmount = store.DebitCardCashBackMaxAmount,
            SnapEBTCAshCashBack = store.SnapEBTCAshCashBack,
            SnapEBTCAshCashBackMaxAmount = store.SnapEBTCAshCashBackMaxAmount,
            MinimumAgeRequired = store.MinimumAgeRequired,
            DisplayChangeDueAfterTender = store.DisplayChangeDueAfterTender,
            DisplayLaneClosed = store.DisplayLaneClosed,
            UseCustomer = store.UseCustomer,

            UseTaxOverride = store.UseTaxOverride,
            UseNoSale = store.UseNoSale,
            UsePaidOut = store.UsePaidOut,
            UseReturn = store.UseReturn,
            UseDiscount = store.UseDiscount,
            UseZeroScale = store.UseZeroScale,
            UseBottleRefund = store.UseBottleRefund,
            UseGiftCards = store.UseGiftCards,
            UseExchange = store.UseExchange,

            UseReprintReceipt = store.UseReprintReceipt,
            UseEBTCheckBalance = store.UseEBTCheckBalance,
            UseRemoveserviceFee = store.UseRemoveserviceFee,

            DefaulBottleDeposit = store.DefaulBottleDeposit,
            PrintReceiptOptional = store.PrintReceiptOptional,
            AutoLogOffCachiers = store.AutoLogOffCachiers,
            AutoEndDate = store.AutoEndDate,
            AutoCloseAllCachiers = store.AutoCloseAllCachiers,
            Theme = store.Theme,
            GiftCardType = store.GiftCardType,


            PrintStoreNameOnReceipt = store.PrintStoreNameOnReceipt,


            //Elastic
            ElasticClient = _configuration["Company:CompanyCode"],
            ElasticStore = store.Name,
            ElasticUrl = _configuration["Serilog:Elasticsearch:nodeUris"],
            ElasticUserName = _configuration["Serilog:Elasticsearch:username"],
            ElasticPassword = _configuration["Serilog:Elasticsearch:password"],
            ElasticLogLevel = 2
        };

        if (device != null)
        {
            _logger.LogInformation("Sending configuration to device {DeviceName}", device.Name);
            await _hub.Clients.Client(device.SignalRConnectionId).SetConfiguration(UpdateWithDevice(newConfig, device));
        }
        else
        {
            //get a device connected and send new configuration
            var devs = await _deviceService.GetAllConnectedByStore(store.Id);

            foreach (var dev in devs)
            {
                _logger.LogInformation("Sending configuration to device {DeviceName}", dev.Name);
                await _hub.Clients.Client(dev.SignalRConnectionId).SetConfiguration(UpdateWithDevice(newConfig, dev));
            }
        }
        return;
    }

    private DeviceConfigurationUpdate UpdateWithDevice(DeviceConfigurationUpdate newConfig, Api.Entities.Device device)
    {
        newConfig.DeviceName = device.Name;
        newConfig.DeviceNumber = device.Id;
        newConfig.PrinterType = device.PrinterType;
        newConfig.PrinterName = device.PrinterName;
        newConfig.LabelPrinterType = device.LabelPrinterType;
        newConfig.LabelPrinterName = device.LabelPrinterName;
        newConfig.ScaleModel = device.ScaleModel;
        newConfig.ScaleComName = device.ScaleComName;
        newConfig.ScaleBaudRate = device.ScaleBaudRate;
        newConfig.PaymentGatewayType = device.PaymentGatewayType;
        newConfig.PaymentGatewayPosLinkDestPort = device.PaymentGatewayPosLinkDestPort;
        newConfig.PaymentGatewayPosLinkCommType = device.PaymentGatewayPosLinkCommType;
        newConfig.PaymentGatewayPosLinkSerialPort = device.PaymentGatewayPosLinkSerialPort;
        newConfig.PaymentGatewayPosLinkBaudRate = device.PaymentGatewayPosLinkBaudRate;
        newConfig.PaymentGatewayPosLinkDestIP = device.PaymentGatewayPosLinkDestIP;
        newConfig.PaymentGatewayPosLinkTimeOut = device.PaymentGatewayPosLinkTimeOut;

        //Elastic search config
        newConfig.ElasticDevice = device.Name;

        return newConfig;
    }
}