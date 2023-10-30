using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Greta.BO.Api.Entities.Events.Internal.OnlineStores;
using Greta.BO.Api.Entities.Lite;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Service;
using Greta.BO.Wix.Handlers.Commands.Categories;
using Greta.BO.Wix.Handlers.Commands.OnlineStores;
using Greta.BO.Wix.Handlers.Commands.Products;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Events.OnlineStores;

/// <inheritdoc />
public class OnlineStoreCreatedHandler : INotificationHandler<OnlineStoreCreated>
{
    private readonly IMediator _mediator;
    private readonly IOnlineStoreService _storeService;
    private readonly ILogger<OnlineStoreCreatedHandler> _logger;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="storeService"></param>
    /// <param name="logger"></param>
    public OnlineStoreCreatedHandler(IMediator mediator, IOnlineStoreService storeService, ILogger<OnlineStoreCreatedHandler> logger)
    {
        _mediator = mediator;
        _storeService = storeService;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task Handle(OnlineStoreCreated notification, CancellationToken cancellationToken = default)
    {
        try
        {
            var refreskToken = await _mediator.Send(new CreateWixOnlineStoreCommand(
                notification.Token),
                cancellationToken);

            var onlineStoreFind = await _storeService.Get(notification.Id);

            if (onlineStoreFind == null)
            {
                _logger.LogError("A problem occurred when trying to associate an online store in Wix");

                throw new BusinessLogicException("A problem occurred when trying to associate an online store in Wix");
            }
            else
            {
                var infoStore = await _mediator.Send(new GetWixOnlineStoreCommand(refreskToken), cancellationToken);

                onlineStoreFind.IsAssociated = true;
                onlineStoreFind.IsActiveWebSite = true;
                onlineStoreFind.RefreshToken = refreskToken;
                onlineStoreFind.Instance = infoStore.Instance.InstanceId;
                onlineStoreFind.Url = infoStore.Site.SiteId;

                if (!notification.IsImport)
                {
                    if (await _mediator.Send(new FinishesWixOnlineStoreCommand(refreskToken), cancellationToken))
                    {
                        await _storeService.Put(notification.Id, onlineStoreFind);
                    };
                }
                else
                {
                    if (await _mediator.Send(new FinishesWixOnlineStoreCommand(refreskToken), cancellationToken))
                    {
                        onlineStoreFind.IsStockUpdated = true;

                        await _storeService.Put(notification.Id, onlineStoreFind);

                        foreach (var department in onlineStoreFind.Departments)
                        {
                            var categories = await _storeService.GetCategoriesWithProduct(department.Id);

                            if (categories.Count > 0)
                            {
                                foreach (var category in categories)
                                {
                                    var cat = await _storeService.GetOnlineCategoryForStore(category.Id, onlineStoreFind.Id);

                                    if (category.AddOnlineStore && cat == null)
                                    {
                                        var wixId = await _mediator.Send(new CreateWixCategoryCommand(
                                            onlineStoreFind.RefreshToken,
                                            LiteCategory.Convert(category, new List<long>())),
                                            cancellationToken);

                                        await _storeService.CreateCategoryOnline(category.Id, onlineStoreFind.Id, wixId);

                                        var catProds = await _storeService.GetCategoryWithProduct(category.Id);

                                        foreach (var product in catProds.Products)
                                        {
                                            foreach (var s in product.StoreProducts)
                                            {
                                                if (s.StoreId == onlineStoreFind.StoreId)
                                                {
                                                    var wixIdProduct = await _mediator.Send(new CreateWixProductCommand(
                                                        onlineStoreFind.RefreshToken,
                                                        LiteProduct.Convert(product, s)),
                                                        cancellationToken);

                                                    await _storeService.CreateProductOnline(
                                                        s.Id,
                                                        onlineStoreFind.Id,
                                                        wixIdProduct);

                                                    var catFind = await _storeService.GetOnlineCategoryForStore(category.Id, onlineStoreFind.Id);

                                                    if (catFind != null)
                                                    {
                                                        await _mediator.Send(new AddWixProductsCommand(
                                                            onlineStoreFind.RefreshToken,
                                                            catFind.OnlineCategoryId,
                                                            new List<string>() { wixIdProduct }),
                                                            cancellationToken);
                                                        _logger.LogInformation("add product to category Wix");
                                                    }
                                                    else
                                                    {
                                                        _logger.LogError("A problem occurred when creating a product in Wix, the category does not exist");
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                _logger.LogError("A problem occurred when creating a product in Wix, the category does not exist");
                            }

                        }
                    }

                }
            }
        }
        catch (System.Exception e)
        {
            _logger.LogError(e, "A problem occurred when trying to associate an online store in Wix");

            throw new BusinessLogicException("A problem occurred when trying to associate an online store in Wix");
        }
    }
}