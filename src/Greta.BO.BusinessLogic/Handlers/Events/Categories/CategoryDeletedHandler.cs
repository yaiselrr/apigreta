using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Greta.BO.Api.Entities.Events.Internal.Categories;
using Greta.BO.BusinessLogic.Service;
using Greta.BO.Wix.Handlers.Commands.Categories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Events.Categories;

/// <inheritdoc />
public class CategoryDeletedHandler : INotificationHandler<CategoryDeleted>
{
    private readonly IMediator _mediator;
    private readonly ICategoryService _service;
    private readonly IOnlineStoreService _storeService;
    private readonly ILogger<CategoryDeletedHandler> _logger;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="service"></param>
    /// <param name="storeService"></param>
    /// <param name="logger"></param>
    public CategoryDeletedHandler(IMediator mediator, ICategoryService service, IOnlineStoreService storeService, ILogger<CategoryDeletedHandler> logger)
    {
        _mediator = mediator;
        _service = service;
        _storeService = storeService;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task Handle(CategoryDeleted notification, CancellationToken cancellationToken = default)
    {
        try
        {
            var onlineReferences = await _service.GetWithOnlineStores(notification.Ids);

            foreach (var oc in onlineReferences.SelectMany(cat => cat.OnlineCategories))
            {
                var category = await _storeService.GetOnlineCategoryByIdForStore(oc.CategoryId);

                if (category != null)
                {
                    await _mediator.Send(new DeleteCommand(
                        oc.OnlineStore.RefreshToken, 
                        oc.OnlineCategoryId), 
                        cancellationToken);
                }
                else
                {
                    _logger.LogError("A problem occurred when deleting a category in Wix, the category does not exist");
                }
            }
        }
        catch (System.Exception e)
        {
            _logger.LogError(e, "A problem occurred when deleting a category in Wix");
        }
    }
}