using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Handlers.Command.StoreProduct;
using Greta.BO.BusinessLogic.Handlers.Command.VendorProduct;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using Greta.BO.BusinessLogic.Specifications.Generics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Command.Products;

/// <summary>
/// Command for create a rapid product
/// </summary>
/// <param name="Product"></param>
public record RapidProductCreateCommand(RapidProductModel Product) : IRequest<RapidProductCreateResponse>, IAuthorizable
{
    /// <summary>
    /// 
    /// </summary>
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"add_edit_{nameof(Product).ToLower()}")
    };
}      

///<inheritdoc/>
public class RapidProductCreateValidator : AbstractValidator<RapidProductCreateCommand>
{
    /// <summary>
    /// 
    /// </summary>
    public RapidProductCreateValidator()
    {
        RuleFor(x => x.Product.Name).NotEmpty().Length(3, 64);
        RuleFor(x => x.Product.CasePack).GreaterThan(0).WithMessage("CasePack is required");
        RuleFor(x => x.Product.CaseCost).GreaterThan(0).WithMessage("CaseCost is required");
        RuleFor(x => x.Product.UnitCost).GreaterThan(0).WithMessage("UnitCost is required");        
        RuleFor(x => x.Product.RetailPrice).GreaterThan(0).WithMessage("RetailPrice is required");
        RuleFor(x => x.Product.DepartmentId).GreaterThan(0).WithMessage("Department id most be more than 0");
        RuleFor(x => x.Product.StoreId).GreaterThan(0).WithMessage("Store id most be more than 0");
        RuleFor(x => x.Product.VendorId).GreaterThan(0).WithMessage("Vendor id most be more than 0");
        RuleFor(x => x.Product.CategoryId).GreaterThan(0).WithMessage("Category id most be more than 0");
        RuleFor(x => x.Product.UPC).Must(UpcConditionalLength).WithMessage("In scale products the UPC must have exactly 5 digits");  
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="command"></param>
    /// <param name="upc"></param>
    /// <returns></returns>
    private bool UpcConditionalLength(RapidProductCreateCommand command, string upc)
    {
        if (command.Product.ProductType == ProductType.SLP) return command.Product.UPC.Length == 5;
        return true;
    }    
}       

///<inheritdoc/>
public class RapidProductCreateHandler : IRequestHandler<RapidProductCreateCommand, RapidProductCreateResponse>
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly ICategoryService _categoryService;
    private readonly IMediator _mediator;
    private readonly IProductService _productService;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="mapper"></param>
    /// <param name="mediator"></param>
    /// <param name="categoryService"></param>
    /// <param name="productService"></param>
    /// <returns></returns>       
    public RapidProductCreateHandler(ILogger<RapidProductCreateHandler> logger, IMapper mapper,
                                     IMediator mediator, IProductService productService, ICategoryService categoryService)
    {
        _logger = logger;
        _mapper = mapper;
        _categoryService = categoryService;        
        _mediator = mediator;
        _productService = productService;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<RapidProductCreateResponse> Handle(RapidProductCreateCommand request, CancellationToken cancellationToken = default)
    {
        var category = await _categoryService.Get(new GetByIdSpec<Api.Entities.Category>(request.Product.CategoryId), cancellationToken);
        var product = _mapper.Map<ProductModel>(await _productService.GetProductByUPCWithStoreAndVendor(request.Product.UPC));
        bool productCreated = false;
        bool storeProductCreated = false;
        bool vendorProductCreated = false;        

        try
        {
            //Create product if not exist

            if (product == null)
            {
                var productCreate = _mapper.Map<ProductModel>(request.Product);
                productCreate.PosVisible = category.VisibleOnPos;
                productCreate.DefaulShelfTagId = category.DefaulShelfTagId;
                productCreate.PromptPriceAtPOS = category.PromptPriceAtPOS;
                productCreate.SnapEBT = category.SnapEBT;
                productCreate.PrintShelfTag = category.PrintShelfTag;
                productCreate.NoPriceOnShelfTag = category.NoPriceOnShelfTag;
                if (category.MinimumAge != null) productCreate.MinimumAge = (int)category.MinimumAge;
                productCreate.AddOnlineStore = category.AddOnlineStore;
                productCreate.Modifier = category.Modifier;
                productCreate.ProductType = ProductType.SPT;

                var resultProduct = await _mediator.Send(new ProductCreate.ProductCommand(productCreate), cancellationToken);

                if (resultProduct == null || resultProduct.Data == null)
                {
                    throw new BusinessLogicException("An error occurred while creating rapid product");
                }

                product = resultProduct.Data;
                productCreated = true;
            }

            //Create store product if not exist

            if (product.StoreProducts.All(x => x.StoreId != request.Product.StoreId))
            {
                var storeProductCreate = new StoreProductCreateModel
                {
                    StoreId = request.Product.StoreId,
                    Price = request.Product.RetailPrice,
                    Cost = request.Product.UnitCost,
                    ProductId = product.Id
                };
                var price = request.Product.RetailPrice;
                storeProductCreate.GrossProfit = request.Product.UnitCost > 0 ? (price - request.Product.UnitCost) / price : 100;

                var resultStoreProduct = await _mediator.Send(new StoreProductCreateCommand(storeProductCreate), cancellationToken);

                if (resultStoreProduct?.Data == null)
                {
                    _logger.LogInformation("An error occurred while creating vendor product from rapid product");
                    throw new BusinessLogicException("An error occurred while creating store product from rapid product");
                }

                storeProductCreated = true;
            }

            //Create vendor product if not exist

            if (product.VendorProducts.All(x => x.VendorId != request.Product.VendorId))
            {
                var vendorProductCreate = new VendorProductModel()
                {
                    VendorId = request.Product.VendorId,
                    ProductCode = request.Product.ProductCode,
                    CaseCost = request.Product.CaseCost,
                    CasePack = request.Product.CasePack,
                    OrderByCase = request.Product.OrderByCase,
                    ProductId = product.Id,
                    UnitCost = request.Product.UnitCost
                };

                var resultVendorProduct = await _mediator.Send(new VendorProductCreate.Command(vendorProductCreate), cancellationToken);

                if (resultVendorProduct == null || resultVendorProduct.Data == null)
                {
                    _logger.LogInformation("An error occurred while creating vendor product from rapid product");
                    throw new BusinessLogicException("An error occurred while creating vendor product from rapid product");
                }

                vendorProductCreated = true;
            }

            // Get product included store and vendor by return

            product = _mapper.Map<ProductModel>(await _productService.GetProductByUPCWithStoreAndVendor(request.Product.UPC));

            //Return Created if product, storeProduct, vendorProduct are created
            //Return Ok if product exist and created VendorProduct and StoreProduct
            //Return NotModified if product exist and exist VendorProduct and StoreProduct
            //Return BusinessLogicException in any other case

            if (productCreated && storeProductCreated && vendorProductCreated)
            {
                return new RapidProductCreateResponse { Data = product, StatusCode = System.Net.HttpStatusCode.Created };
            }
            else if (!productCreated && (storeProductCreated || vendorProductCreated))
            {
                return new RapidProductCreateResponse { Data = product, StatusCode = System.Net.HttpStatusCode.OK };
            }
            else if (!productCreated)
            {
                return new RapidProductCreateResponse { Data = product, StatusCode = System.Net.HttpStatusCode.NotModified };
            }
            else
            {
                throw new BusinessLogicException("An error occurred while creating rapid product");
            }
        }
        catch (BusinessLogicException ex)
        {
            _logger.LogInformation(ex.Message);
            throw new BusinessLogicException(ex.Message);
        }
    }
}

///<inheritdoc/>
public record RapidProductCreateResponse : CQRSResponse<ProductModel>;

