using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Greta.BO.Api.Entities.Events.Internal.OnlineStores;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Command.OnlineStore;

/// <summary>
/// Change the state of the entity
/// </summary>
/// <param name="Ids">Entity Ids</param>
public record OnlineStoreDisassociateCommand(List<long> Ids) : IRequest<OnlineStoreDisassociateResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"add_edit_{nameof(Store).ToLower()}")
    };
}

/// <inheritdoc />
public class OnlineStoreDisassociateHandler : IRequestHandler<OnlineStoreDisassociateCommand, OnlineStoreDisassociateResponse>
{
    private readonly ILogger _logger;
    private readonly IOnlineStoreService _service;
    private readonly IMediator _mediator;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    /// <param name="mediator"></param>
    public OnlineStoreDisassociateHandler(ILogger<OnlineStoreDisassociateHandler> logger, IOnlineStoreService service, IMediator mediator)
    {
        _logger = logger;
        _service = service;
        _mediator = mediator;
    }

    /// <inheritdoc />
    public async Task<OnlineStoreDisassociateResponse> Handle(OnlineStoreDisassociateCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            await _mediator.Publish(new OnlineStoreDeleted(request.Ids), cancellationToken);
            _logger.LogInformation("The online store was successfully disassociated");
            return new OnlineStoreDisassociateResponse { Data = true };
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error when trying to disassociate an online store");
            throw new BussinessValidationException(new List<string>() { e.Message });
        }
    }
}

/// <inheritdoc />
public record OnlineStoreDisassociateResponse : CQRSResponse<bool>;