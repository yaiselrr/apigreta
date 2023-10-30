using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using Greta.Sdk.LabelConverter;
using Greta.Sdk.LabelConverter.models;
using MediatR;
using Newtonsoft.Json;

namespace Greta.BO.BusinessLogic.Handlers.Command.Zpl;

public record ProcessBatchCommand(long StoreId, long ModelId, long LabelId) : IRequest<ProcessBatchResponse>;

public record ProcessBatchResponse : CQRSResponse<List<string>>;

public class ProcessBatchHandler : IRequestHandler<ProcessBatchCommand, ProcessBatchResponse>
{
    private readonly IStoreService _storeService;
    private readonly IScaleLabelTypeService _scaleLabelTypeService;
    private readonly IPriceBatchDetailService _priceBatchService;

    public ProcessBatchHandler(
        IStoreService storeService,
        IScaleLabelTypeService scaleLabelTypeService,
        IPriceBatchDetailService priceBatchService
    )
    {
        _storeService = storeService;
        _scaleLabelTypeService = scaleLabelTypeService;
        _priceBatchService = priceBatchService;
    }

    public async Task<ProcessBatchResponse> Handle(ProcessBatchCommand request,
        CancellationToken cancellationToken)
    {
        var store = await _storeService.Get(request.StoreId);
        if (store == null)
            throw new BussinessValidationException("Store Batch not found.");
        var labelType = await _scaleLabelTypeService.Get(request.LabelId);

        var batch = await _priceBatchService.GetFullDetails(request.ModelId);

        if (batch == null || batch.Count == 0)
            throw new BussinessValidationException("Price Batch not found or empty.");

        //Convert all price batch on zpl
        var zpls = new List<string>();
        string stringEmpty = "_";
        foreach (var x in batch.Where(x => x.Product != null))
        {
            var temp = new ShelfTagHolderModel()
            {
                Date = DateTime.Now.ToString("MM/dd/yyyy"),
                UPC = x.Product.UPC,
                RetailPrice = x.Price.ToString("C2", CultureInfo.CurrentCulture),
                VendorProductCode = stringEmpty,
                ProductName = x.Product.Name,
                VendorName = stringEmpty,
                CategoryName = x.Product.Category.Name,
                DepartmentName = x.Product.Department.Name,
                VendorCasePack = stringEmpty,
                CashDiscountServiceFee =
                    (x.Price + (store.CashDiscount ? (x.Price * store.CashDiscountValue / 100) : 0)).ToString(
                        "C2", CultureInfo.CurrentCulture),
            };
            var label = labelType != null ? JsonConvert.DeserializeObject<LabelDesign>(labelType.Design) : null;
            if (label != null)
            {
                var r = temp.ToZpl(label);
                zpls.Add(r);
            }
            else
            {
                if (x.Product != null && x.Product.DefaulShelfTag != null)
                {
                    var tLabel = JsonConvert.DeserializeObject<LabelDesign>(x.Product.DefaulShelfTag.Design);
                    var r1 = temp.ToZpl(tLabel);
                    zpls.Add(r1);
                }
            }
        }

        foreach (var pb in batch.Where(x => x.Family != null))
        {
            foreach (var p in pb.Family.Products)
            {
                var temp = new ShelfTagHolderModel()
                {
                    Date = DateTime.Now.ToString("MM/dd/yyyy"),
                    UPC = p.UPC,
                    RetailPrice = pb.Price.ToString("C2", CultureInfo.CurrentCulture),
                    VendorProductCode = stringEmpty,
                    ProductName = p.Name,
                    VendorName = stringEmpty,
                    CategoryName = p.Category.Name,
                    DepartmentName = p.Department.Name,
                    VendorCasePack = stringEmpty,
                    CashDiscountServiceFee =
                        (pb.Price + (store.CashDiscount ? (pb.Price * store.CashDiscountValue / 100) : 0))
                        .ToString(
                            "C2", CultureInfo.CurrentCulture),
                };
                var label = labelType != null
                    ? JsonConvert.DeserializeObject<LabelDesign>(labelType.Design)
                    : null;
                if (label != null)
                {
                    var r = temp.ToZpl(label);
                    zpls.Add(r);
                }
                else
                {
                    if (p.DefaulShelfTag != null)
                    {
                        var tLabel =
                            JsonConvert.DeserializeObject<LabelDesign>(p.DefaulShelfTag.Design);
                        var r1 = temp.ToZpl(tLabel);
                        zpls.Add(r1);
                    }
                }
            }
        }

        return new ProcessBatchResponse() { Data = zpls };
    }
}