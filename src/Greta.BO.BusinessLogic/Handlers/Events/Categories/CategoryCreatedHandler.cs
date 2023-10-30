using System.Threading;
using System.Threading.Tasks;
using Greta.BO.Api.Entities.Events.Internal.Categories;
using Greta.BO.BusinessLogic.Service;
using Greta.BO.Wix.Handlers.Commands.Categories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Events.Categories;

/// <inheritdoc />
public class CategoryCreatedHandler : INotificationHandler<CategoryCreated>
{
    private readonly IMediator _mediator;
    private readonly IOnlineStoreService _storeService;
    private readonly ILogger<CategoryCreatedHandler> _logger;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="storeService"></param>
    /// <param name="logger"></param>
    public CategoryCreatedHandler(IMediator mediator, IOnlineStoreService storeService, ILogger<CategoryCreatedHandler> logger)
    {
        _mediator = mediator;
        _storeService = storeService;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task Handle(CategoryCreated notification, CancellationToken cancellationToken = default)
    {
        try
        {
            var stores = await _storeService.GetWixStoreTokens(
                null, 
                notification.Category.DepartmentId, 
                cancellationToken);

            foreach (var store in stores)
            {
                var category = await _storeService.GetOnlineCategoryForStore(
                    notification.Category.CategoryId, 
                    store.Id);

                if (category == null)
                {
                    var wixId = await _mediator.Send(new CreateWixCategoryCommand(
                        store.RefreshToken, 
                        notification.Category),
                        cancellationToken);

                    await _storeService.CreateCategoryOnline(
                        notification.Category.Id, 
                        store.Id, 
                        wixId);
                }
                else
                {
                    _logger.LogError("A problem occurred when deleting a category in Wix, the category already exists");
                }
            }
        }
        catch (System.Exception e)
        {
            _logger.LogError(e, "A problem occurred when deleting a category in Wix");
        }
    }
}