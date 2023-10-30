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

/// <summary>
/// Process a product to zpl
/// </summary>
/// <param name="Model"></param>
public record ProcessProductToZplCommand(ProductToZplModel Model) : IRequest<ProcessProductToZplResponse>;

/// <summary>
/// Process a product to zpl response
/// </summary>
public record ProcessProductToZplResponse : CQRSResponse<List<string>>;

/// <summary>
/// handler for process a product to zpl handler
/// </summary>
public class ProcessProductToZplHandler : IRequestHandler<ProcessProductToZplCommand, ProcessProductToZplResponse>
{
    private readonly IStoreService _storeService;
    private readonly IScaleLabelTypeService _scaleLabelTypeService;
    private readonly IStoreProductService _storeProductService;
    private readonly IMediator _mediator;
    
    public ProcessProductToZplHandler(
        IStoreService storeService,
        IScaleLabelTypeService scaleLabelTypeService,
        IStoreProductService storeProductService,
        IMediator mediator
    )
    {
        _storeService = storeService;
        _scaleLabelTypeService = scaleLabelTypeService;
        _storeProductService = storeProductService;
        _mediator = mediator;
    }
    /// <inheritdoc />
    public async Task<ProcessProductToZplResponse> Handle(ProcessProductToZplCommand request,
        CancellationToken cancellationToken)
    {
        var store = await _storeService.Get(request.Model.StoreId);
        if (store == null)
            throw new BussinessValidationException("Store Batch not found.");

        var labelType = (request.Model.TagId.HasValue || request.Model.TagId > 0)
            ? await _scaleLabelTypeService.Get(request.Model.TagId.Value)
            : null;

        var product =
            await _storeProductService.GetAllByProductAndStore(request.Model.ProductId, request.Model.StoreId);
        if (product == null)
            throw new BussinessValidationException("Store Product not found.");
        const string stringEmpty = "_";
        var temp = new ShelfTagHolderModel()
        {
            Date = DateTime.Now.ToString("MM/dd/yyyy"),
            UPC = product.Product.UPC,
            RetailPrice = product.Price.ToString("C2", CultureInfo.CurrentCulture),
            VendorProductCode = stringEmpty,
            ProductName = product.Product.Name,
            VendorName = stringEmpty,
            CategoryName = product.Product.Category.Name,
            DepartmentName = product.Product.Department.Name,
            VendorCasePack = stringEmpty,
            CashDiscountServiceFee =
                (product.Price + (store.CashDiscount ? (product.Price * store.CashDiscountValue / 100) : 0)).ToString(
                    "C2", CultureInfo.CurrentCulture),
        };

        var label = labelType != null ? JsonConvert.DeserializeObject<LabelDesign>(labelType.Design) : null;
        if (label == null)
        {
            if (product.Product is { DefaulShelfTag: not null })
            {
                label = JsonConvert.DeserializeObject<LabelDesign>(product.Product.DefaulShelfTag.Design);
            }
        }

        if (label == null)
        {
            throw new BussinessValidationException("Label design not found.");
        }

        var tempZpl = await _mediator.Send(new CreateShelfTagZplCodeCommand(
            temp,
            label,
            request.Model.QtyToPrint
        ), cancellationToken);

        return new ProcessProductToZplResponse() { Data = tempZpl };
    }
}