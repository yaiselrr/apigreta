using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Models.Dto.Search;
using Greta.BO.BusinessLogic.Service;
using Greta.Sdk.EFCore.Middleware;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Command.Inventory;

/// <summary>
/// Create suggested order
/// </summary>
/// <param name="StoreId"></param>
/// <param name="VendorId"></param>
/// <param name="Filter"></param>
public record InventoryCreateSuggestedOrderCommand(long StoreId, long VendorId, InventorySearchModel Filter) : IRequest<InventoryCreateSuggestedOrderResponse>, IAuthorizable
{
    /// <summary>
    /// 
    /// </summary>
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement("add_edit_vendor_order")
    };
}

///<inheritdoc/>
public record InventoryCreateSuggestedOrderResponse: CQRSResponse<long>;

///<inheritdoc/>
public class InventoryCreateSuggestedOrderHandler:IRequestHandler<InventoryCreateSuggestedOrderCommand, InventoryCreateSuggestedOrderResponse>
{
    private readonly ILogger _logger;
    private readonly IStoreProductService _storeProductService;
    private readonly IVendorOrderService _vendorOrderService;
    private readonly IBOUserService _userService;
    private readonly IAuthenticateUser<string> _auth;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="storeProductService"></param>
    /// <param name="vendorOrderService"></param>
    /// <param name="userService"></param>
    /// <param name="auth"></param>
    public InventoryCreateSuggestedOrderHandler(ILogger<InventoryCreateSuggestedOrderHandler> logger, IStoreProductService storeProductService, 
                                                IVendorOrderService vendorOrderService, IBOUserService userService, IAuthenticateUser<string> auth)
    {
        _logger = logger;
        _storeProductService = storeProductService;
        _vendorOrderService = vendorOrderService;
        _userService = userService;
        _auth = auth;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<InventoryCreateSuggestedOrderResponse> Handle(InventoryCreateSuggestedOrderCommand request, CancellationToken cancellationToken = default)
    {
        try
        {
            //get suggested products
            var details = await _storeProductService.FilterSuggested(request.Filter, request.StoreId, request.VendorId);
            
            var user = await _userService.GetByUserId(_auth.UserId);
            
            //create vendor order
            var order = new Api.Entities.VendorOrder()
            {
                VendorId = request.VendorId,
                StoreId = request.StoreId,
                ReceivedDate = request.Filter.CreationDate ?? DateTime.UtcNow,
                
                VendorOrderDetails = details.Select( d => new Api.Entities.VendorOrderDetail()
                {
                    ProductId = d.ProductId,
                    QuantityOnHand = d.QtyHand,
                    ProductCode = d.Product.VendorProducts[0].ProductCode,
                    UPC = d.Product.UPC,
                    CasePack = d.Product.VendorProducts[0].CasePack,
                    CaseCost = d.Product.VendorProducts[0].CaseCost,
                    OrderAmount = d.OrderAmount
                    
                }).ToList(),
                UserId = user.Id,
                OrderedDate = null,
            };

            var res = await _vendorOrderService.Post(order);

            return new InventoryCreateSuggestedOrderResponse() { Data = res.Id };
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            throw new BusinessLogicException("An exception occurred while creating a suggested order");
        }
    }
}