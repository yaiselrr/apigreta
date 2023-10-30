using System.Threading;
using System.Threading.Tasks;
using Greta.BO.Api.Entities.Events.Internal.Categories;
using Greta.BO.BusinessLogic.Service;
using Greta.BO.Wix.Handlers.Commands.Categories;
using Greta.BO.Api.Entities.Lite;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Collections.Generic;
using Greta.BO.Wix.Handlers.Commands.Products;

namespace Greta.BO.BusinessLogic.Handlers.Events.Categories;

/// <inheritdoc />
public class CategoryUpdatedHandler : INotificationHandler<CategoryUpdated>
{
    private readonly IMediator _mediator;
    private readonly IOnlineStoreService _storeService;
    private readonly IProductService _productService;
    private readonly ILogger<CategoryUpdatedHandler> _logger;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="storeService"></param>
    /// <param name="productService"></param>
    /// <param name="logger"></param>
    public CategoryUpdatedHandler(IMediator mediator, IOnlineStoreService storeService, IProductService productService, ILogger<CategoryUpdatedHandler> logger)
    {
        _mediator = mediator;
        _storeService = storeService;
        _productService = productService;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task Handle(CategoryUpdated notification, CancellationToken cancellationToken = default)
    {
        try
        {
            var stores = await _storeService.GetWixStoreTokens(
                null, 
                notification.Category.DepartmentId, 
                cancellationToken);

            foreach (var store in stores)
            {
                var onlineCategoryId = await _storeService.GetOnlineCategoryIdForStore(
                    notification.Category.Id, 
                    store.Id);

                if (onlineCategoryId != null)
                {
                    await _mediator.Send(new UpdateWixCategoryCommand(
                        store.RefreshToken, onlineCategoryId, 
                        notification.Category), 
                        cancellationToken);
                }
                else
                {
                    var wixId = await _mediator.Send(new CreateWixCategoryCommand(
                        store.RefreshToken, 
                        notification.Category), 
                        cancellationToken);

                    await _storeService.CreateCategoryOnline(
                        notification.Category.Id, 
                        store.Id, 
                        wixId);

                    foreach (var product in notification.Products)
                    {
                        foreach (var s in product.StoreProducts)
                        {
                            if (s.StoreId == store.StoreId)
                            {
                                var wixIdProduct = await _mediator.Send(new CreateWixProductCommand(
                                    store.RefreshToken, 
                                    LiteProduct.Convert(product, s)), 
                                    cancellationToken);

                                await _storeService.CreateProductOnline(
                                    s.Id, 
                                    store.Id, 
                                    wixIdProduct);

                                var cat = await _storeService.GetOnlineCategoryForStore(
                                    notification.Category.Id, 
                                    store.Id);

                                if (cat != null)
                                {
                                    await _mediator.Send(new AddWixProductsCommand(
                                        store.RefreshToken, 
                                        cat.OnlineCategoryId, 
                                        new List<string>() { wixIdProduct }), 
                                        cancellationToken);
                                }
                                else
                                {
                                    _logger.LogError("A problem occurred when creating a product in Wix, the category does not exist");
                                }

                                _logger.LogInformation("add product to category Wix");
                            }
                        }

                        await _productService.UpdateProductOnline(product.Id);
                    }
                }
            }
        }
        catch (System.Exception e)
        {
            _logger.LogError(e, "A problem occurred when creating a category in Wix");
        }
    }
}