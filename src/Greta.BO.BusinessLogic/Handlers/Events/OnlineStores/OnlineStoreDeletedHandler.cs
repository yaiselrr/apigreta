using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Greta.BO.Api.Entities.Events.Internal.OnlineStores;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Service;
using Greta.BO.Wix.Handlers.Commands.OnlineStores;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Events.OnlineStores;

/// <inheritdoc />
public class OnlineStoreDeletedHandler : INotificationHandler<OnlineStoreDeleted>
{
    private readonly IMediator _mediator;
    private readonly IOnlineStoreService _storeService;
    private readonly ILogger<OnlineStoreDeletedHandler> _logger;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="storeService"></param>
    /// <param name="logger"></param>
    public OnlineStoreDeletedHandler(IMediator mediator, IOnlineStoreService storeService, ILogger<OnlineStoreDeletedHandler> logger)
    {
        _mediator = mediator;
        _storeService = storeService;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task Handle(OnlineStoreDeleted notification, CancellationToken cancellationToken = default)
    {
        try
        {
            foreach (var id in notification.Ids)
            {
                var onlineStoreFind = await _storeService.Get(id);

                if (onlineStoreFind == null)
                {
                    _logger.LogError("A problem occurred when trying to disassociate an online store in Wix");

                    throw new BusinessLogicException("A problem occurred when trying to disassociate an online store in Wix");
                }
                else
                { 
                    onlineStoreFind.IsAssociated = false;
                    onlineStoreFind.Isdeleted = true;

                    await _storeService.Put(id, onlineStoreFind);

                    // await _mediator.Send(new DeleteWixOnlineStoreCommand(
                    //     onlineStoreFind.RefreshToken,
                    //     new List<string>() { onlineStoreFind.Url }),
                    //     cancellationToken);
                }
            }
        }
        catch (System.Exception e)
        {
            _logger.LogError(e, "A problem occurred when trying to disassociate an online store in Wix");

            throw new BusinessLogicException("A problem occurred when trying to disassociate an online store in Wix");
        }
    }
}