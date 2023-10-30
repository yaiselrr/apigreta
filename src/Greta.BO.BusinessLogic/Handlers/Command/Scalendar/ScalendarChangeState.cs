using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Command.Scalendar;

/// <summary>
/// Change the state of the entity
/// </summary>
/// <param name="Id">Entity Id</param>
/// <param name="State">State to change</param>
public record ScalendarChangeStateCommand(long Id, bool State) : IRequest<ScalendarChangeStateResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"add_edit_{nameof(Scalendar).ToLower()}")
    };
}

/// <inheritdoc />
public class ScalendarChangeStateHandler : IRequestHandler<ScalendarChangeStateCommand, ScalendarChangeStateResponse>
{
    private readonly ILogger _logger;
    private readonly IScalendarService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    public ScalendarChangeStateHandler(ILogger<ScalendarChangeStateHandler> logger, IScalendarService service)
    {
        _logger = logger;
        _service = service;
    }

    /// <inheritdoc />
    public async Task<ScalendarChangeStateResponse> Handle(ScalendarChangeStateCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _service.ChangeState(request.Id, request.State);
        _logger.LogInformation("Entity with id {RequestId} state change to {RequestState}", request.Id, request.State);
        return new ScalendarChangeStateResponse { Data = result };
    }
}

/// <inheritdoc />
public record ScalendarChangeStateResponse : CQRSResponse<bool>;