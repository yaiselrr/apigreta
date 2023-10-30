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
/// <param name="Id">Entity Id</param>
/// <param name="Token">Token</param>
/// <param name="IsImport">IsImport</param>
public record OnlineStoreAssociateCommand(long Id, string Token, bool IsImport) : IRequest<OnlineStoreAssociateResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"add_edit_{nameof(Store).ToLower()}")
    };
}

/// <inheritdoc />
public class OnlineStoreAssociateHandler : IRequestHandler<OnlineStoreAssociateCommand, OnlineStoreAssociateResponse>
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
    public OnlineStoreAssociateHandler(ILogger<OnlineStoreAssociateHandler> logger, IOnlineStoreService service, IMediator mediator)
    {
        _logger = logger;
        _service = service;
        _mediator = mediator;
    }

    /// <inheritdoc />
    public async Task<OnlineStoreAssociateResponse> Handle(OnlineStoreAssociateCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            await _mediator.Publish(new OnlineStoreCreated(request.Id, request.Token, request.IsImport), cancellationToken);
            _logger.LogInformation("The online store was successfully associated");
            return new OnlineStoreAssociateResponse { Data = true };
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error when trying to associate an online store");
            throw new BussinessValidationException(new List<string>() { e.Message });
        }
    }
}

/// <inheritdoc />
public record OnlineStoreAssociateResponse : CQRSResponse<bool>;