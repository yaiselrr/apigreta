using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Command.Tax;

/// <summary>
/// Change state of entity
/// </summary>
/// <param name="Id">Entity id</param>
/// <param name="State">New state</param>
public record TaxChangeStateCommand(long Id, bool State) : IRequest<TaxChangeStateResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"add_edit_{nameof(Tax).ToLower()}")
    };
}

/// <inheritdoc />
public class TaxChangeStateHandler : IRequestHandler<TaxChangeStateCommand, TaxChangeStateResponse>
{
    private readonly ILogger _logger;
    private readonly ITaxService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    public TaxChangeStateHandler(ILogger<TaxChangeStateHandler> logger, ITaxService service)
    {
        _logger = logger;
        _service = service;
    }

    /// <inheritdoc />
    public async Task<TaxChangeStateResponse> Handle(TaxChangeStateCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _service.ChangeState(request.Id, request.State);
        _logger.LogInformation("Entity with id {RequestId} state change to {RequestState}", request.Id, request.State);
        return new TaxChangeStateResponse { Data = result };
    }
}

/// <inheritdoc />
public record TaxChangeStateResponse : CQRSResponse<bool>;