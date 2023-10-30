using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Command.ExternalScale;

/// <summary>
/// Change the state of the entity
/// </summary>
/// <param name="Id">Entity Id</param>
/// <param name="State">State to change</param>
public record ExternalScaleChangeStateCommand(long Id, bool State) : IRequest<ExternalScaleChangeStateResponse>,
    IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"add_edit_external_scale")
    };
}

/// <inheritdoc />
public class
    ExternalScaleChangeStateHandler : IRequestHandler<ExternalScaleChangeStateCommand, ExternalScaleChangeStateResponse>
{
    private readonly ILogger _logger;
    private readonly IExternalScaleService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    public ExternalScaleChangeStateHandler(ILogger<ExternalScaleChangeStateHandler> logger,
        IExternalScaleService service)
    {
        _logger = logger;
        _service = service;
    }

    /// <inheritdoc />
    public async Task<ExternalScaleChangeStateResponse> Handle(ExternalScaleChangeStateCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _service.ChangeState(request.Id, request.State);
        _logger.LogInformation("Entity with id {RequestId} state change to {RequestState}", request.Id, request.State);
        return new ExternalScaleChangeStateResponse { Data = result };
    }
}

/// <inheritdoc />
public record ExternalScaleChangeStateResponse : CQRSResponse<bool>;