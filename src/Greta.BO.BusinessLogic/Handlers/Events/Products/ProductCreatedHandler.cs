using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Greta.BO.Api.Entities.Events.Internal.Products;
using Greta.BO.BusinessLogic.Service;
using Greta.BO.Wix.Handlers.Commands.Categories;
using Greta.BO.Wix.Handlers.Commands.Products;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Events.Products;

/// <inheritdoc />
public class ProductCreatedHandler : INotificationHandler<ProductCreated>
{
    private readonly ILogger<ProductCreatedHandler> _logger;
    private readonly IMediator _mediator;
    private readonly IOnlineStoreService _storeService;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="mediator"></param>
    /// <param name="storeService"></param>
    public ProductCreatedHandler(
        ILogger<ProductCreatedHandler> logger,
        IMediator mediator,
        IOnlineStoreService storeService)
    {
        _logger = logger;
        _mediator = mediator;
        _storeService = storeService;
    }

    /// <inheritdoc />
    public async Task Handle(ProductCreated notification, CancellationToken cancellationToken)
    {
        try
        {
            var stores = await _storeService.GetWixStoreTokens(
                notification.Stores, 
                notification.Product.DepartmentId, 
                cancellationToken);

            foreach (var store in stores)
            {
                var productOnline = await _storeService.GetOnlineProductForStore(
                    notification.Product.Id, 
                    store.Id);

                if (productOnline == null)
                {
                    var wixId = await _mediator.Send(new CreateWixProductCommand(
                            store.RefreshToken,
                            notification.Product),
                            cancellationToken);

                    await _storeService.CreateProductOnline(
                        notification.StoreProductReferenceId, 
                        store.Id, 
                        wixId);
                    
                    var cat = await _storeService.GetOnlineCategoryForStore(
                        notification.Product.CategoryId, 
                        store.Id);

                    if (cat != null)
                    {
                        await _mediator.Send(new AddWixProductsCommand(
                            store.RefreshToken,
                            cat.OnlineCategoryId,
                            new List<string>() { wixId }),
                            cancellationToken);
                    }
                    else
                    {
                        _logger.LogError("A problem occurred when creating a product in Wix, the category does not exist");
                    }
                }
                else
                {
                    _logger.LogError("A problem occurred when creating a product in Wix, the product does not exist");
                }
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "A problem occurred when creating a product in Wix");
        }
    }
}