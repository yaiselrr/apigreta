using System.Threading;
using System.Threading.Tasks;
using Greta.BO.Api.Entities.Events.Internal.Products;
using Greta.BO.BusinessLogic.Service;
using Greta.BO.Wix.Handlers.Commands.Products;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Events.Products;

/// <inheritdoc />
public class ProductChangeStateHandler : INotificationHandler<ProductChangeState>
{
    private readonly IMediator _mediator;
    private readonly IOnlineStoreService _storeService;
    private readonly ILogger<ProductChangeStateHandler> _logger;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="storeService"></param>
    /// <param name="logger"></param>
    public ProductChangeStateHandler(IMediator mediator, IOnlineStoreService storeService, ILogger<ProductChangeStateHandler> logger)
    {
        _mediator = mediator;
        _storeService = storeService;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task Handle(ProductChangeState notification, CancellationToken cancellationToken = default)
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
                    await _mediator.Send(new ChangeStateWixProductCommand(
                        store.RefreshToken, 
                        productOnline.Result.OnlineProductId, 
                        notification.Product, 
                        notification.Product.AddOnlineStore), 
                        cancellationToken);
                }
                else
                {
                    if (notification.Product.AddOnlineStore)
                    {
                        _logger.LogError("A problem occurred when enabling a product in Wix");
                    }
                    else
                    {
                        _logger.LogError("A problem occurred when disabling a product in Wix");
                    }
                }
            }
        }
        catch (System.Exception e)
        {
            if (notification.Product.AddOnlineStore)
            {
                _logger.LogError(e, "A problem occurred when enabling a product in Wix");
            }
            else
            {
                _logger.LogError(e, "A problem occurred when disabling a product in Wix");
            }
        }
    }
}