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
public class ProductUpdatedHandler : INotificationHandler<ProductUpdated>
{
    private readonly IMediator _mediator;
    private readonly IOnlineStoreService _storeService;
    private readonly ILogger<ProductUpdatedHandler> _logger;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="storeService"></param>
    /// <param name="logger"></param>
    public ProductUpdatedHandler(IMediator mediator, IOnlineStoreService storeService, ILogger<ProductUpdatedHandler> logger)
    {
        _mediator = mediator;
        _storeService = storeService;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task Handle(ProductUpdated notification, CancellationToken cancellationToken = default)
    {
        try
        {
            var stores = await _storeService.GetWixStoreTokens(
                notification.Stores,
                notification.Product.DepartmentId,
                cancellationToken);

            foreach (var store in stores)
            {
                var productOnline = _storeService.GetOnlineProductForStore(
                    notification.StoreProductReferenceId,
                    store.Id);

                if (productOnline.Result != null)
                {
                    var cat = await _storeService.GetOnlineCategoryForStore(
                        notification.Product.CategoryId,
                        store.Id);

                    var olCat = await _storeService.GetOnlineCategoryForStore(
                        notification.OldCategory,
                        store.Id);

                    if (cat != null && cat.Id != olCat.Id)
                    {
                        await _mediator.Send(new UpdateWixProductCommand(
                             store.RefreshToken,
                             productOnline.Result.OnlineProductId,
                             notification.Product),
                             cancellationToken);

                        await _mediator.Send(new AddWixProductsCommand(
                            store.RefreshToken,
                            cat.OnlineCategoryId,
                            new List<string>() { productOnline.Result.OnlineProductId }),
                            cancellationToken);


                        await _mediator.Send(new RemoveProductsFromCollectionCommand(
                            store.RefreshToken,
                            olCat.OnlineCategoryId,
                            new List<string>() { productOnline.Result.OnlineProductId }),
                            cancellationToken);
                    }
                    else if (cat != null && cat.Id == olCat.Id)
                    {
                        var wixId = await _mediator.Send(new UpdateWixProductCommand(
                             store.RefreshToken,
                             productOnline.Result.OnlineProductId,
                             notification.Product),
                             cancellationToken);
                    }
                    else
                    {
                        await _mediator.Send(new ChangeStateWixProductCommand(
                            store.RefreshToken,
                            productOnline.Result.OnlineProductId,
                            notification.Product,
                            false),
                            cancellationToken);
                    }
                }
                else
                {

                    var cat = await _storeService.GetOnlineCategoryForStore(
                        notification.Product.CategoryId,
                        store.Id);

                    if (cat != null)
                    {
                        var wixId = await _mediator.Send(new CreateWixProductCommand(
                            store.RefreshToken,
                            notification.Product),
                            cancellationToken);

                        await _storeService.CreateProductOnline(
                            notification.StoreProductReferenceId,
                            store.Id,
                            wixId);

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
                    _logger.LogError("A problem occurred when updating a product in Wix");
                }
            }
        }
        catch (System.Exception e)
        {
            _logger.LogError(e, "A problem occurred when updating a product in Wix");
        }
    }
}