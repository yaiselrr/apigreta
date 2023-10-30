using System.Threading;
using System.Threading.Tasks;
using Greta.BO.Api.Entities.Events.Internal.Products;
using Greta.BO.BusinessLogic.Service;
using Greta.BO.Wix.Handlers.Commands.Products;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Events.Products;

/// <inheritdoc />
public class ProductDeletedHandler : INotificationHandler<ProductDeleted>
{
    private readonly IMediator _mediator;
    private readonly IOnlineStoreService _service;
    private readonly ILogger<ProductDeletedHandler> _logger;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="service"></param>
    /// <param name="logger"></param>
    public ProductDeletedHandler(IMediator mediator, IOnlineStoreService service, ILogger<ProductDeletedHandler> logger)
    {
        _mediator = mediator;
        _service = service;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task Handle(ProductDeleted notification, CancellationToken cancellationToken = default)
    {
        try
        {
            var stores = await _service.GetWixStoreTokens(
                notification.Stores, 
                notification.Product.DepartmentId, 
                cancellationToken);

            foreach (var store in stores)
            {
                var productOnline = _service.GetOnlineProductForStore(
                    notification.StoreProductReferenceId, 
                    store.Id);

                if (productOnline != null)
                {
                    await _mediator.Send(new DeleteProductCommand(
                        store.RefreshToken, 
                        productOnline.Result.OnlineProductId), 
                        cancellationToken);
                }
                else
                {
                    _logger.LogError("A problem occurred when deleting a product in Wix, the product does not exist");
                }
            }
        }
        catch (System.Exception e)
        {
            _logger.LogError(e, "A problem occurred when deleting a product in Wix");
        }

    }
}