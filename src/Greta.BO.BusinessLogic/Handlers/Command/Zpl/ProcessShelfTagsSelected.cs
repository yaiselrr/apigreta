using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using Greta.Sdk.LabelConverter.models;
using MediatR;
using Newtonsoft.Json;

namespace Greta.BO.BusinessLogic.Handlers.Command.Zpl;

public record ProcessShelfTagsSelectedCommand
    (ProcessShelfTagSelectedModel Model) : IRequest<ProcessShelfTagsSelectedResponse>;

public record ProcessShelfTagsSelectedResponse : CQRSResponse<ShelfTagPrintResponse>;

public class
    ProcessShelfTagsSelectedHandler : IRequestHandler<ProcessShelfTagsSelectedCommand, ProcessShelfTagsSelectedResponse>
{
    private readonly IStoreService _storeService;
    private readonly IScaleLabelTypeService _scaleLabelTypeService;
    private readonly IProductService _productService;
    private readonly IShelfTagService _shelfTagService;
    private readonly IMediator _mediator;

    public ProcessShelfTagsSelectedHandler(
        IStoreService storeService,
        IScaleLabelTypeService scaleLabelTypeService,
        IProductService productService,
        IShelfTagService shelfTagService,
        IMediator mediator
    )
    {
        _storeService = storeService;
        _scaleLabelTypeService = scaleLabelTypeService;
        _productService = productService;
        _shelfTagService = shelfTagService;
        _mediator = mediator;
    }

    public async Task<ProcessShelfTagsSelectedResponse> Handle(ProcessShelfTagsSelectedCommand request,
        CancellationToken cancellationToken)
    {
        var store = await _storeService.Get(request.Model.StoreId);
        if (store == null)
            throw new BussinessValidationException("Store not found.");
        var labelType = (request.Model.TagId != -1) ? await _scaleLabelTypeService.Get(request.Model.TagId) : null;

        if (request.Model.ShelfTagIds == null || request.Model.ShelfTagIds.Count == 0)
            throw new BussinessValidationException("Shelf tags to print not found or is empty.");

        //Convert all shelf tags on zpl
        var zpls = new List<string>();
        var idsPrinted = new List<long>();
        string stringEmpty = "_";
        foreach (var shelfTagId in request.Model.ShelfTagIds)
        {
            var x = await _shelfTagService.Get(shelfTagId);
            var temp = new ShelfTagHolderModel()
            {
                Date = DateTime.Now.ToString("MM/dd/yyyy"),
                UPC = x.UPC,
                RetailPrice = x.Price.ToString("C2", CultureInfo.CurrentCulture),

                VendorProductCode = !string.IsNullOrEmpty(x.ProductCode) ? x.ProductCode : stringEmpty,
                ProductName = x.ProductName,
                VendorName = !string.IsNullOrEmpty(x.VendorName) ? x.VendorName : stringEmpty,
                CategoryName = !string.IsNullOrEmpty(x.CategoryName) ? x.CategoryName : stringEmpty,
                DepartmentName = !string.IsNullOrEmpty(x.DepartmentName) ? x.DepartmentName : stringEmpty,

                VendorCasePack = x.CasePack.ToString(),
                CashDiscountServiceFee =
                    (x.Price + (store.CashDiscount ? (x.Price * store.CashDiscountValue / 100) : 0)).ToString(
                        "C2", CultureInfo.CurrentCulture),
            };
            var label = labelType != null ? JsonConvert.DeserializeObject<LabelDesign>(labelType.Design) : null;
            if (label == null)
            {
                var product = await _productService.GetProductByIdWithDefaultShelfTag(x.ProductId);
                if (product != null && product.DefaulShelfTag != null)
                {
                    label = JsonConvert.DeserializeObject<LabelDesign>(product.DefaulShelfTag.Design);
                }
            }

            if (label == null)
            {
                throw new BussinessValidationException("Label design not found.");
            }

            var tempZpl = await _mediator.Send(new CreateShelfTagZplCodeCommand(
                temp,
                label,
                x.QTYToPrint
            ), cancellationToken);
            idsPrinted.Add(x.Id);

            zpls.AddRange(tempZpl);
        }

        return new ProcessShelfTagsSelectedResponse()
            { Data = new ShelfTagPrintResponse() { Zpls = zpls, IdsShelfTagPrinted = idsPrinted } };
    }
}

/// <summary>
/// Shelf tag print inner response
/// </summary>
public class ShelfTagPrintResponse
{
    /// <summary>
    /// Zpl code for all shelf tags
    /// </summary>
    public List<string> Zpls { get; set; }
    
    /// <summary>
    /// Ids of shelf tags printed
    /// </summary>
    public List<long> IdsShelfTagPrinted { get; set; }
}