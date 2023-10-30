using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Entities.Dto;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.BusinessLogic.Extensions;
using Greta.BO.BusinessLogic.Handlers.Command;
using Greta.BO.BusinessLogic.Handlers.Command.Customer;
using Greta.BO.BusinessLogic.Handlers.Command.Device;
using Greta.BO.BusinessLogic.Handlers.Command.GiftCardCommands;
using Greta.BO.BusinessLogic.Handlers.Command.Synchro;
using Greta.BO.BusinessLogic.Handlers.Queries.Employee;
using Greta.BO.BusinessLogic.Handlers.Queries.Store;
using Greta.BO.BusinessLogic.Handlers.Queries.Synchro;
using Greta.BO.BusinessLogic.Models.Hubs;
using Greta.BO.BusinessLogic.Options;
using Greta.BO.BusinessLogic.Service;
using Greta.Identity.Api.EventContracts.Auth;
using Greta.Sdk.ExternalScale.Enums;
using Greta.Sdk.FileStorage.Interfaces;
using Greta.Sdk.Hangfire.MediatR;
using Greta.Sdk.MassTransit.Contracts;
using Greta.Sdk.Printing.Models;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SignalRSwaggerGen.Attributes;
using SignalRSwaggerGen.Enums;

namespace Greta.BO.BusinessLogic.Hubs;

public class SynchroPathData
{
    public long Tag { get; set; }
    public string Path { get; set; }
}

[Serializable]
public class StoreInfoModel
{
    public string Store { get; set; }
    public string Address1 { get; set; }
    public string Device { get; set; }
    public long DeviceNumber { get; set; }
    public string Phone { get; set; }
    public string Zip { get; set; }
}

[Serializable]
public class RealTimeStatus
{
    public DateTime Date { get; set; }
    public List<ProcessInformation> Process { get; set; }
}

[Serializable]
public class ProcessInformation
{
    public string ProcessName { get; set; }
    public string Version { get; set; }
    public int ProcessId { get; set; }
    public DateTime StartTime { get; set; }
}


public class SingleCommand
{
    public Command Command { get; set; }
    public string Args { get; set; }
}

[SignalRHub(autoDiscover: AutoDiscover.MethodsAndParams, path: "/cloudhub")]
public interface ICloudHub
{
    /// <summary>
    ///     Inform to the Client need a update
    /// </summary>
    /// <returns></returns>
    [SignalRMethod(autoDiscover: AutoDiscover.Params, description: "Inform to the Client need a update")]
    Task OnNeedUpdate(string connectionId, List<SynchroPathData> pathsS);

    /// <summary>
    ///     Get a list of printers on the system
    /// </summary>
    [SignalRMethod(autoDiscover: AutoDiscover.Params, description: "Get a list of printers on the system")]
    Task GetPrinters(string type, long deviceid, string connectionId);

    /// <summary>
    ///     SetConfiguration
    /// </summary>
    /// <param name="configuration">Send Configuration to device</param>
    [SignalRMethod(autoDiscover: AutoDiscover.Params, description: "SetConfiguration")]
    Task SetConfiguration(DeviceConfigurationUpdate configuration);

    /// <summary>
    /// Test print from device
    /// </summary>
    /// <param name="printer"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    [SignalRMethod(autoDiscover: AutoDiscover.Params, description: "Test print from device")]
    Task TestPrint(string printer, string data);

    /// <summary>
    /// Notify to clients the backup is ready
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    [SignalRMethod(autoDiscover: AutoDiscover.Params, description: "Notify to clients the backup is ready")]
    Task OnCompleteFullBackup(NotifyWorkerFullBackupStatus data);

    //send commands to device
    //Close: close opened app
    //CleanCache: remove cache for load data
    //Logout: close all apps y logout user
    //Reboot: Restart machine
    Task<bool> SingleCommandExecute(SingleCommand command);

    /// <summary>
    /// send external scale data to device
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    Task SendExternalScaleExternalJobToDevice(SendScaleDataDto data);

    Task CanGetDirectoryFiles(string path);
}

public class HolderSale
{
    public List<Sale> Sales { get; set; }
}

public class TemporalApiHolder
{
    public string Code { get; set; }
    public string Secret { get; set; }
    public string UserId { get; set; }
}

[SignalRHub(path: "/cloudhub")]
public class CloudHub : Hub<ICloudHub>
{
    private readonly IHubContext<FrontHub, IFrontHub> _frontHubContext;
    private readonly IStorageProvider _storage;
    private readonly IStoreProductService _storeProductService;
    private readonly IMediator _mediator;
    private readonly ICustomerService _customerService;
    private readonly IExternalJobService _externalJobService;
    private readonly IExternalScaleService _externalScaleService;
    private readonly ILoyaltyDiscountService _loyaltyDiscountService;
    private readonly IChangePriceReasonCodeService _priceReasonCodeService;
    private readonly IRequestClient<AddTemporalyApiKeyContract> _addTemporallyApiKeyClient;
    private readonly ITimeKeepingService _timeKeepingService;
    private readonly MainOption _options;
    private readonly ILogger<CloudHub> _logger;
    private readonly IDeviceService _deviceService;
    private readonly ISaleService _saleService;
    private readonly IBOUserService _userService;
    private readonly IBatchCloseService _batchCloseService;
    private readonly IGiftCardService _giftCardService;

    public CloudHub(
        ILogger<CloudHub> logger,
        IDeviceService deviceService,
        ISaleService saleService,
        IBOUserService userService,
        IBatchCloseService batchCloseService,
        IGiftCardService giftCardService,
        IHubContext<FrontHub, IFrontHub> frontHubContext,
        IStorageProvider storage,
        IStoreProductService storeProductService,
        IMediator mediator,
        ICustomerService customerService,
        IExternalJobService externalJobService,
        IExternalScaleService externalScaleService,
        ILoyaltyDiscountService loyaltyDiscountService,
        IChangePriceReasonCodeService priceReasonCodeService,
        IRequestClient<AddTemporalyApiKeyContract> addTemporallyApiKeyClient,
        IOptions<MainOption> options,
        ITimeKeepingService timeKeepingService
    )
    {
        _logger = logger;
        _deviceService = deviceService;
        _saleService = saleService;
        _userService = userService;
        _batchCloseService = batchCloseService;
        _giftCardService = giftCardService;
        _frontHubContext = frontHubContext;
        _storage = storage;
        _storeProductService = storeProductService;
        _mediator = mediator;
        _customerService = customerService;
        _externalJobService = externalJobService;
        _externalScaleService = externalScaleService;
        _loyaltyDiscountService = loyaltyDiscountService;
        _priceReasonCodeService = priceReasonCodeService;
        _addTemporallyApiKeyClient = addTemporallyApiKeyClient;
        _timeKeepingService = timeKeepingService;
        _options = options.Value;
    }

    public async Task<List<string>> GetUserInformation(string deviceId, long employeeId)
    {
        var device = await _deviceService.GetByDeviceLic(deviceId);
        if (device == null)
        {
            _logger.LogInformation($"GetUserInformation fail, device not found");
            return new List<string>();
        }

        var response = await _mediator.Send(new EmployeeNotificationsQuery(device.StoreId, employeeId));

        return response.Data;
    }

    public async Task<string> ClockIn(string deviceId, long employeeId, string employeeName, DateTime date,
        string formatDate)
    {
        var device = await _deviceService.GetByDeviceLic(deviceId);
        if (device == null)
        {
            _logger.LogInformation($"ClockIn fail, device not found");
            return null;
        }
        
        return await _timeKeepingService.ClockIn(employeeId, employeeName,device.Id, device.StoreId, device.Store.Name, date, formatDate);
    }

    public async Task<string> ClockOut(string deviceId, long employeeId, string employeeName, DateTime date,
        string formatDate)
    {
        var device = await _deviceService.GetByDeviceLic(deviceId);
        if (device == null)
        {
            _logger.LogInformation($"ClockOut fail, device not found");
            return null;
        }
        
        return await _timeKeepingService.ClockOut(employeeId, employeeName, device.Id, device.StoreId, device.Store.Name, date, formatDate);
    }

    public async Task<TemporalApiHolder> GetApiHolder(string deviceId, long userId)
    {
        try
        {
            var device = await _deviceService.GetByDeviceLic(deviceId);
            if (device == null)
            {
                _logger.LogInformation($"GetApiHolder fail, device not found");
                return null;
            }

            var user = await _userService.GetById(userId);
            if (user == null)
            {
                _logger.LogInformation($"GetApiHolder fail, user not found");
                return null;
            }

            //addTemporalyApiKeyClient
            var data = await _addTemporallyApiKeyClient.GetResponse<AddTemporalyApiKeyResponse, FailResponseContract>(new
            {
                UserId = user.UserId,
                __Header_application = _options.CompanyCode,
                __Header_user = user.UserId,
            });
            if (data.Is(out Response<FailResponseContract> responseFail))
            {
                _logger.LogError("GetApiHolder fail, {Message}",
                    new List<string>(responseFail.Message.ErrorMessages)[0]);
                return null;
            }

            if (data.Is(out Response<AddTemporalyApiKeyResponse> response))
            {
                return new TemporalApiHolder()
                {
                    Secret = response.Message.Secret,
                    Code = _options.CompanyCode,
                    UserId = user.UserId
                };
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error on  NotifyScaleChangePrice");
        }

        return null;
    }

    public async Task<bool> NotifyScaleChangePrice(string deviceId, List<ChangePriceReasonCode> reasons)
    {
        try
        {
            var device = await _deviceService.GetByDeviceLic(deviceId);
            if (device == null)
            {
                return false;
            }

            _logger.LogInformation($"Receive {reasons.Count} Change price from device {device.Name}");
            foreach (var reason in reasons)
            {
                reason.DeviceId = device.Id;
                reason.DeviceName = device.Name;
                await _priceReasonCodeService.Post(reason);
            }

            _logger.LogInformation(
                $"Complete synchronization of {reasons.Count} Change price from device {device.Name}");
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error on  NotifyScaleChangePrice");
        }

        return false;
    }

    public async Task<bool> NotifyScaleUpdateStatus(string deviceId, long job, int status, string message)
    {
        try
        {
            var device = await _deviceService.GetByDeviceLic(deviceId);
            if (device == null)
            {
                return false;
            }

            // update the status of one job
            //if the job is scale job then send notification to frontend
            var task = await _externalJobService.Get(job);

            task.Status = (ExternalJobStatus)status;
            task.Messages = message;
            if (task.Status == ExternalJobStatus.Fail)
            {
                task.FailRetry++;
                task.Messages = $"Fail retry ({task.FailRetry}/5)";
                _logger.LogWarning("Fail to process job {ProcessId} retry {Retry}/5", job, task.FailRetry);
            }

            await _externalJobService.Put(task.Id, task);
            bool notify = true;
            if (task.Type == ExternalJobType.ScaleUpdate)
            {
                if (task.Status == ExternalJobStatus.Complete)
                {
                    if (task.RawData != null)
                    {
                        var split = task.RawData.Split("-");

                        //ExternalScaleOperationType
                        try
                        {
                            var esid = long.Parse(split[0]);
                            var type = (ExternalScaleOperationType)int.Parse(split[1]);

                            var externalScale = await _externalScaleService.Get(esid);
                            switch (type)
                            {
                                case ExternalScaleOperationType.Department:
                                    externalScale.LastDepartmentUpdate = DateTime.UtcNow;
                                    break;
                                case ExternalScaleOperationType.Category:
                                    externalScale.LastCategoryUpdate = DateTime.UtcNow;
                                    break;
                                case ExternalScaleOperationType.Product:
                                    externalScale.LastPluUpdate = DateTime.UtcNow;
                                    break;
                            }

                            if (externalScale.SyncDeviceId != device.Id)
                            {
                                //dont send notification to frontend if the device is not the sync device
                                notify = false;
                            }

                            await _externalScaleService.Put(externalScale.Id, externalScale);
                        }
                        catch
                        {
                            // ignored
                        }
                    }
                }

                _logger.LogInformation(
                    "Receive update of scale sync for job {JobId} with status {Status} and message {Message}", job,
                    task.Status.ToString(), message);

                if (task.FailRetry >= 5)
                {
                    await _frontHubContext.Clients.All.OnUpdateScaleStatus(job, status, "CompleteFail");
                    return true;
                }

                if (notify)
                    //Send notification to interface about the new state of this task.
                    await _frontHubContext.Clients.All.OnUpdateScaleStatus(job, status, message);
            }

            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<List<DeviceSmallModel>> GetDevicesName(string deviceId)
    {
        var device = await _deviceService.GetByDeviceLic(deviceId);
        if (device == null)
        {
            return null;
        }

        var devices = await _deviceService.Get();
        _logger.LogInformation("Found {DevicesCount} devices on get devices name", devices.Count);
        return devices.Select(x => new DeviceSmallModel()
        {
            Id = x.Id,
            Name = x.Name
        }).ToList();
    }

    public async Task<SalesSearchResponseDto> SearchSale(string deviceId, SalesSearchRequestDto filter)
    {
        var device = await _deviceService.GetByDeviceLic(deviceId);
        if (device == null)
        {
            _logger.LogError("Search Sale. Device not found.");
            return new SalesSearchResponseDto() { Error = true, Message = "Device not found." };
        }

        if (filter == null)
        {
            _logger.LogError("Search Sale. Filter object not found.");
            return new SalesSearchResponseDto() { Error = true, Message = "Filter object not found." };
        }

        try
        {
            _logger.LogInformation("Begin Search");
            var result = await _saleService.GetPosFilteredSale(device, filter);
            _logger.LogInformation($"End Search {result.Count} sales found");
            return new SalesSearchResponseDto()
            {
                Sales = result
            };
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error to process search sale for reprint, from device {DeviceName}", device.Name);
            return new SalesSearchResponseDto() { Error = true, Message = "Error searching sales. Try again." };
        }
    }

    public async Task<RecipeModel> GetPrintableSale(string deviceId, long saleId)
    {
        //Add logs
        var device = await _deviceService.GetByDeviceLic(deviceId);
        if (device == null)
        {
            return null;
        }

        try
        {
            _logger.LogInformation($"Begin GetPrintableSale for sale id {saleId}");
            //get a sale by id and convert to receipt model for print.
            var sale = await _saleService.Get(saleId);
            if (sale == null) return null;
            var model = sale.ToReceiptModel(device);
            _logger.LogInformation($"End GetPrintableSale {model.InvoiceNumber} sale found");
            return model;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error to process get sale for reprint from device {DeviceName}", device.Name);
            return null;
        }
    }

    public async Task<Customer> SearchCustomer(string deviceId, string phone)
    {
        var device = await _deviceService.GetByDeviceLic(deviceId);
        if (device == null)
        {
            return null;
        }

        if (phone == null)
        {
            return null;
        }

        _logger.LogInformation($"Search Customer for number {phone}");
        var customer = await _customerService.SearchByPhone(phone);
        if (customer == null)
        {
            _logger.LogInformation($"Customer for number {phone} not found.");
            return null;
        }

        if (customer.Discounts != null)
        {
            customer.Discounts = customer.Discounts.Select(x => new Discount()
            {
                Id = x.Id,
                Type = x.Type,
                Value = x.Value,
                Name = x.Name,
                ApplyAutomatically = x.ApplyAutomatically,
                ApplyToProduct = x.ApplyToProduct,
                State = x.State
            }).ToList();
        }

        if (customer.MixAndMatches != null)
        {
            customer.MixAndMatches = customer.MixAndMatches.Select(x => new MixAndMatch() { Id = x.Id }).ToList();
        }

        _logger.LogInformation("Customer for number {Phone} found {FirstName}", phone, customer.FirstName);
        //process points
        var discount = await _loyaltyDiscountService.GetByStore(device.StoreId);
        if (discount != null)
        {
            // customer.DiscountAvailable = customer.StoreCredit * discount.Value;
            // customer.DiscountValue = customer.StoreCredit;
            // if (customer.DiscountAvailable > discount.Maximum)
            // {
            //     customer.DiscountAvailable = discount.Maximum;
            //     customer.DiscountValue = (int) (customer.DiscountAvailable / discount.Value);
            // }
            customer.DiscountMaximum = discount.Maximum;
            customer.DiscountValue = discount.Value;
        }

        return customer;
    }

    public async Task<StoreInfoModel> GetStoreInfo(string deviceId)
    {
        var device = await _deviceService.GetByDeviceLic(deviceId);
        if (device == null)
        {
            return null;
        }

        return new StoreInfoModel()
        {
            Store = device.Store.Name,
            Address1 = device.Store.Address,
            Device = device.Name,
            DeviceNumber = device.Id,
            Phone = device.Store.Phone
        };
    }

    public async Task<GiftCardSaleResponse> GiftCardMakeSale(
        string deviceId,
        string card,
        decimal amount,
        long employeeId,
        string employeeName
    )
    {
        var device = await _deviceService.GetByDeviceLic(deviceId);
        if (device == null)
        {
            return new GiftCardSaleResponse()
            {
                Status = GiftCardRequestStatus.Declined
            };
        }

        _logger.LogInformation(
            $"Device {device.Name} send sale query for card end with {card.Substring(card.Length - 4)}");

        var date = DateTime.UtcNow;
        var obj = await _giftCardService.GetByCardNumber(card);
        //GiftCardRequestStatus
        if (obj == null)
        {
            _logger.LogInformation(
                $"Send card not found to device {device.Name} for card end with {card.Substring(card.Length - 4)}");
            return new GiftCardSaleResponse()
            {
                Status = GiftCardRequestStatus.NotFound
            };
        }

        if (amount > 0 && obj.Balance < amount)
        {
            _logger.LogInformation(
                $"Send card not fund to device {device.Name} for card end with {card.Substring(card.Length - 4)}");
            return new GiftCardSaleResponse()
            {
                RemainAmount = obj.Balance,
                Status = GiftCardRequestStatus.NotFund
            };
        }

        obj.Balance -= amount;
        obj.LastUsed = date;
        var transaction = new GiftCardTransaction()
        {
            Amount = amount,
            DateUse = date,
            EmployeeId = employeeId,
            EmployeeName = employeeName,
            Device = device.Id,
            StoreId = device.StoreId,
            GiftCardId = obj.Id
        };
        long upd = await _giftCardService.AddTransaction(obj, transaction);
        _logger.LogInformation(
            $"Transaction receive from device {device.Name} for card end with {card.Substring(card.Length - 4)}, register sucessfully.");
        return new GiftCardSaleResponse()
        {
            ApprovedAmount = amount,
            RemainAmount = obj.Balance,
            Status = GiftCardRequestStatus.Approved,
            GiftCardTransactionId = upd
        };
    }

    public async Task<string> GiftCardCheckBalance(string deviceId, string card)
    {
        var device = await _deviceService.GetByDeviceLic(deviceId);
        if (device == null)
        {
            return "Unauthorized device.";
        }

        var obj = await _giftCardService.GetByCardNumber(card);
        if (obj == null)
        {
            return "Gift Card Not Found.";
        }

        if (obj.Balance <= 0)
        {
            return "Gift card without funds";
        }

        return obj.Balance.ToString("C2");
    }

    /// <summary>
    /// Get all paths update for device
    /// </summary>
    /// <param name="deviceId"></param>
    /// <param name="tags"></param>
    /// <returns></returns>
    public async Task<List<SynchroPathData>> GetPartialsUpdates(string deviceId, long tags)
    {
        _logger.LogInformation("Device {Id} request partials path", deviceId);
        var device = await _deviceService.GetByDeviceLic(deviceId);
        if (device != null)
        {
            var paths = await _mediator.Send(new GetPathsLeftForDeviceQuery(device.DeviceId, device.SynchroVersion,
                device.StoreId));
            return paths //.Where(x => x.Tag > tags)
                .Select(x => new SynchroPathData() { Path = x.FilePath, Tag = x.Tag }).ToList();
        }
        else
        {
            return new List<SynchroPathData>();
        }
    }

    public async Task<bool> PartialSynchroComplete(string deviceId, long newTag)
    {
        var device = await _deviceService.GetByDeviceLic(deviceId);
        if (device == null)
        {
            _logger.LogError(
                "Device with id {DeviceId} not found when try to update the synchronization to version {Tag}", deviceId,
                newTag);
            return false;
        }

        _logger.LogInformation("Device {DeviceName} update to version with tag {NewTag}", device.Name, newTag);
        return await _mediator.Send(new DeviceUpdateTagVersionCommand(deviceId, newTag));
    }


    private byte[] GetZipArchive(byte[] data, string filename)
    {
        using (var archiveStream = new MemoryStream(data))
        {
            using (var archive = new ZipArchive(archiveStream))
            {
                var entry = archive.Entries.FirstOrDefault();
                if (entry != null)
                {
                    using (var unzippedEntryStream = entry.Open())
                    {
                        using (var ms = new MemoryStream())
                        {
                            unzippedEntryStream.CopyTo(ms);
                            var unzippedArray = ms.ToArray();

                            return unzippedArray;
                        }
                    }
                }
            }
        }

        return null;
    }

    public async Task<bool> UploadCloseBatch(string deviceId, BatchCloseDto batchClose) //List<Sale> sales)
    {
        var device = await _deviceService.GetByDeviceLic(deviceId);
        if (device == null)
        {
            _logger.LogError($"Device not found.");
            return false;
        }

        try
        {
            await _batchCloseService.Post(new BatchClose()
            {
                DeviceId = device.Id,
                AcquirerName = batchClose.AcquirerName,
                EBTAmount = batchClose.EBTAmount,
                Batch = batchClose.Batch,
                BatchRecordCount = int.Parse(batchClose.BatchRecordCount),
                HostResponseText = batchClose.HostResponseText,
                HostTotalsAmount1 = decimal.Parse(batchClose.HostTotalsAmount1),
                HostTotalsAmount5 = decimal.Parse(batchClose.HostTotalsAmount5),
                HostTotalsCount1 = int.Parse(batchClose.HostTotalsCount1),
                HostTotalsCount5 = int.Parse(batchClose.HostTotalsCount5),
                MerchantName = batchClose.MerchantName,
                TerminalNumber = batchClose.TerminalNumber,
                TransactionResponse = batchClose.TransactionResponse,
            });
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error storing the batchclose");
            return false;
        }
    }

    public async Task<List<long>> UploadSales(
        string deviceId,
        string sales
    )
    {
        var ids = new List<long>();
        try
        {
            var device = await _deviceService.GetByDeviceLic(deviceId);
            if (device == null)
            {
                _logger.LogError($"Device not found");
                return null;
            }


            //download file
            var data = await _storage.Download(sales);
            var unzipped = GetZipArchive(data, "uploadsales.sales");
            //read and decode file
            using var stream = new MemoryStream(unzipped);
            var obj = await JsonSerializer.DeserializeAsync(stream, typeof(HolderSale)) as HolderSale;
            if (obj == null)
            {
                _logger.LogError($"Sales file dont have a correct data");
                return null;
            }

            //process information
            _logger.LogInformation("Device {DeviceName} upload sales count {SalesCount}", device.Name, obj.Sales.Count);

            //remove file
            try
            {
                await _storage.DeleteFile(sales);
            }
            catch
            {
                // ignored
            }

            //update qty holder
            List<StoreProduct> qtySales = new List<StoreProduct>();
            List<GiftCard> giftcards = new List<GiftCard>();
            List<Customer> customers = new List<Customer>();
            foreach (var s in obj.Sales)
            {
                List<StoreProduct> qtySalesT = new List<StoreProduct>();
                try
                {
                    if (!await _saleService.ExistSale(s.Invoice, s.SaleTime, s.SubTotal))
                    {
                        List<GiftCard> giftCardsTemp = new List<GiftCard>();
                        s.LocalId = s.Id;
                        s.Id = 0;
                        s.DeviceId = device.Id;
                        s.StoreId = device.StoreId;
                        s.SaleTime = s.CreatedAt;

                        var haveCustomer = s.CustomerId != null && s.CustomerId > 0;
                        var customerLoyaltyPoints = 0;

                        //if customer apply for a discount with point of loyalty them hear i remove the points
                        if (s.CustomerDiscountAmount > 0)
                        {
                            customerLoyaltyPoints -= s.CustomerDiscountPointsUsed;
                        }

                        foreach (var t in s.Tenders)
                        {
                            t.Id = 0;
                            t.SaleId = 0;
                            t.Sale = null;

                            //Todo: Remove when Poslink error was found
                            if (
                                t.Name != "Check" &&
                                t.Name != "Cash" &&
                                t.Amount > 0 &&
                                t.ApprovedAmount == 0 &&
                                t.RawResponse == null &&
                                t.CardType == null &&
                                t.Pan == null &&
                                t.ResultCode == null &&
                                t.ResultTxt == null
                            )
                            {
                                t.ApprovedAmount = t.Amount;
                                t.Pan = "XXXXXXXXXXXX0000";
                            }
                        }

                        foreach (var t in s.Products)
                        {
                            customerLoyaltyPoints += t.LoyaltyPoints * (int)t.QTY;
                            if (t.Name == "Gift Card")
                            {
                                giftCardsTemp.Add(new GiftCard()
                                {
                                    SaleId = 0,
                                    Number = t.UPC,
                                    Amount = t.Price,
                                    Balance = t.Price,
                                    LastUsed = DateTime.UtcNow,
                                    DateSold = s.SaleTime,
                                    EmployeeId = s.EmployeeId,
                                    EmployeeName = s.Username,
                                    DeviceId = s.DeviceId,
                                    StoreId = s.StoreId,
                                    Transactions = new List<GiftCardTransaction>()
                                });
                            }

                            t.Id = 0;
                            t.SaleId = 0;
                            t.Sale = null;

                            var qytTemp = qtySalesT.FirstOrDefault(x => x.ProductId == t.ProductId);
                            if (qytTemp == null)
                            {
                                qtySalesT.Add(new StoreProduct() { QtyHand = t.QTY, ProductId = t.ProductId });
                            }
                            else
                            {
                                qytTemp.QtyHand += t.QTY;
                            }

                            //update cost ??????
                            t.Cost = await _storeProductService.GetCostByProductAndStore(device.StoreId,
                                t.ProductId);
                        }

                        //s.Taxs = new List<SaleTax>();
                        foreach (var t in s.Taxs)
                        {
                            t.Id = 0;
                            t.SaleId = 0;
                            t.Sale = null;
                            t.SaleProduct = null;
                        }

                        if (s.Discounts != null)
                            foreach (var t in s.Discounts)
                            {
                                t.Id = 0;
                                t.SaleId = 0;
                                t.Sale = null;
                            }

                        //s.Fees = new List<SaleFee>();
                        foreach (var t in s.Fees)
                        {
                            t.Id = 0;
                            t.SaleId = 0;
                            t.Sale = null;
                        }
                        
                        foreach (var t in s.SaleProductRemoveds)
                        {
                            t.Id = 0;
                            t.SaleId = 0;
                            t.Sale = null;
                        }

                        var user = await _userService.GetById(s.EmployeeId);
                        if (user != null)
                        {
                            s.UserCreatorId = (user.UserId);
                        }

                        //if exist one sale with same invoice and the same sale time dont update


                        var ns = await _saleService.Post(s);

                        foreach (var p in qtySalesT)
                        {
                            var qytTemp = qtySales.FirstOrDefault(x => x.ProductId == p.ProductId);
                            if (qytTemp == null)
                            {
                                qtySales.Add(new StoreProduct() { QtyHand = p.QtyHand, ProductId = p.ProductId });
                            }
                            else
                            {
                                qytTemp.QtyHand += p.QtyHand;
                            }
                        }

                        //process gift card
                        foreach (var bg in giftCardsTemp)
                        {
                            bg.SaleId = ns.Id;
                        }

                        if (giftCardsTemp.Count > 0)
                            giftcards.AddRange(giftCardsTemp);

                        if (haveCustomer && s.CustomerId.HasValue)
                        {
                            var cValue = s.CustomerId.Value;
                            var ec = customers
                                .FirstOrDefault(x => x.Id == cValue);
                            if (ec != null)
                            {
                                ec.StoreCredit += customerLoyaltyPoints;
                                ec.LastBuy = s.SaleTime;
                            }
                            else
                            {
                                customers.Add(new Customer
                                {
                                    Id = s.CustomerId.Value,
                                    StoreCredit = customerLoyaltyPoints,
                                    LastBuy = s.SaleTime
                                });
                            }
                        }
                    }
                    else
                    {
                        _logger.LogInformation("Ignoring sale {SaleId} with invoice number {Invoice}", s.Id, s.Invoice);
                    }

                    ids.Add(s.LocalId);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Error processing sale {LocalId} with invoice number {Invoice}", s.LocalId,
                        s.Invoice);
                }
            }

            try
            {
                //process the customers
                await _mediator.Publish(new CustomerUpdatePoints.Command(customers));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error processing CustomerPoints of sale");
            }

            try
            {
                //process giftcards
                await _mediator.Publish(new ProcessCredictCardsCommand(giftcards));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error processing GiftCard of sale");
            }

            try
            {
                //update qty on hand the warning is ok dont worry
                await _mediator.Publish(new ProcessProductsQtyOnSales.Command(qtySales, device.StoreId));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error processing Products of sale");
            }

            _logger.LogInformation("Sending {Count} ids for mark to device {DeviceName}", ids.Count, device.Name);
            return ids;
        }
        catch (Exception e)
        {
            _logger.LogInformation(e, $"Error processing sales");
            return ids;
        }
    }

    /// <summary>
    ///     Update last connection time for device.
    /// </summary>
    /// <returns></returns>
    [SignalRMethod(autoDiscover: AutoDiscover.Params, description: "Update last connection time for device.")]
    public async Task<bool> PingPong(string deviceId)
    {
        return await _deviceService
            .UpdatePong(deviceId, Context.ConnectionId);
    }

    public async Task<bool> PingPongFull(string deviceId, RealTimeStatus realTimeStatus)
    {
        //Process Realtime status from the device
        var device = await _deviceService.GetByDeviceLic(deviceId);
        if (device == null)
        {
            _logger.LogError($"Device not found.");
            return false;
        }

        var brokerData = realTimeStatus.Process.FirstOrDefault(x => x.ProcessName == "Greta.Desktop.Broker");
        device.BrokerAlive = brokerData != null;
        device.BrokerVersion = brokerData?.Version;
        var workerData = realTimeStatus.Process.FirstOrDefault(x => x.ProcessName == "Greta.Desktop.Worker");
        device.WorkerAlive = workerData != null;
        device.WorkerVersion = workerData?.Version;
        device.SignalRConnectionId = Context.ConnectionId;
        device.RealTimeRaw = JsonSerializer.Serialize(realTimeStatus.Process);
        device.LastPongTime = DateTime.UtcNow;

        return await _deviceService
            .Put(device.Id, device);
    }

    /// <summary>
    ///     Notify the response of device when request the printers
    /// </summary>
    /// <param name="printers"></param>
    /// <param name="deviceId"></param>
    /// <param name="connectionId"></param>
    /// <returns></returns>
    [SignalRMethod(autoDiscover: AutoDiscover.Params,
        description: "Notify the response of device when request the printers")]
    public async Task<bool> OnGetPrinters(List<string> printers, long deviceId, string connectionId)
    {
        _logger.LogInformation("Receive printers from device {DeviceId}", deviceId);
        //update databaseand send to requested client send
        var device = await _deviceService.Get(deviceId);
        device.Printers = string.Join(",", printers);
        await _deviceService.Put(deviceId, device);
        _logger.LogInformation("Data receive {Printers} sent to {ConnectionId}", device.Printers, connectionId);
        if (connectionId == null)
            await _frontHubContext.Clients.All.OnGetPrinter(new
            {
                DeviceId = deviceId,
                device.Printers
            });
        else
            await _frontHubContext.Clients.Client(connectionId).OnGetPrinter(new { deviceId, device.Printers });

        return true;
    }

    [SignalRMethod(autoDiscover: AutoDiscover.Params, description: "RequestFullBackup")]
    public async Task<string> RequestFullBackup(string guid)
    {
        var storeGuid = Guid.Parse(guid);
        var response = await _mediator.Send(new StoreGetByGuidQuery(storeGuid));
        var store = response.Data;
        if (store == null)
        {
            _logger.LogError("Failed to create full backup for store {Guid}. Store not found", guid);
            throw new Exception("Store not found");
        }

        _logger.LogInformation("Received FullBackup request for store {StoreName}", store.Name);
        //if (store.LastBackupPath != null && !store.LastBackupPath.Equals(""))
        //    if (store.SynchroVersion - 2 <= store.LastBackupVersion &&
        //        store.LastBackupTime > DateTime.Now.AddDays(-1))
        //    {
        //        _logger.LogInformation($"Using five days cache full backup in {store.LastBackupPath}");
        //        _logger.LogInformation(
        //            "Sending notification to the devices involved. (Not Implemented Yet)");
        //        return store.LastBackupPath;
        //    }
        _mediator.EnqueueNew(new SynchroFullBackupCommand(storeGuid, ConnectionId: Context.ConnectionId));
        return null;
    }

    [SignalRMethod(autoDiscover: AutoDiscover.Params, description: "OnNotifyWorkerFullBackupStatus")]
    public async Task OnNotifyWorkerFullBackupStatus(NotifyWorkerFullBackupStatus data)
    {
        await Clients.Client(data.ConnectionId).OnCompleteFullBackup(data);
    }

    [SignalRMethod(autoDiscover: AutoDiscover.Params, description: "OnDeviceConnected")]
    public async Task<bool> OnDeviceConnected(string deviceId)
    {
        var device = await _deviceService
            .SignalRConnected(deviceId, Context.ConnectionId);

        if (device == null)
            return false;

        _logger.LogInformation("OnDeviceConnected from device {DeviceName}", device.Name);

        return true;
    }

    [SignalRMethod(autoDiscover: AutoDiscover.Params, description: "OnDeviceDisconnected")]
    public async Task OnDeviceDisconnected(string deviceId)
    {
        var connect = await _deviceService
            .SignalRDisconnected(
                Context.ConnectionId);
        if (connect != null)
            _logger.LogInformation("OnDeviceDisconnected from device {DeviceId}", deviceId);
        else
            _logger.LogInformation("OnDeviceDisconnected from fail from device {DeviceId}", deviceId);
    }

    public async Task<bool> OnConfigurationReceive(string deviceId)
    {
        var device = await _deviceService
            .SignalRConnected(deviceId, Context.ConnectionId);

        if (device == null)
            return false;
        _logger.LogInformation("Device {DeviceName} send a configuration received message", device.Name);
        return true;
    }

    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
        //adding the is to storage
        // var userId = Context.ConnectionId;
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        await base.OnDisconnectedAsync(exception);
        //remove the is to storage
        // var userId = Context.ConnectionId;
        await _deviceService
            .SignalRDisconnected(
                Context.ConnectionId);
    }
}