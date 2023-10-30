using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Command.ScaleCategory;

/// <summary>
/// Change state of entity
/// </summary>
/// <param name="Id">Entity id</param>
/// <param name="State">State to change</param>
public record ScaleCategoryChangeStateCommand(long Id, bool State) : IRequest<ScaleCategoryChangeStateResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"add_edit_scale_ScaleCategory")
    };
}

/// <inheritdoc />
public class ScaleCategoryChangeStateHandler : IRequestHandler<ScaleCategoryChangeStateCommand, ScaleCategoryChangeStateResponse>
{
    private readonly ILogger _logger;
    private readonly IScaleCategoryService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    public ScaleCategoryChangeStateHandler(ILogger<ScaleCategoryChangeStateHandler> logger, IScaleCategoryService service)
    {
        _logger = logger;
        _service = service;
    }

    /// <inheritdoc />
    public async Task<ScaleCategoryChangeStateResponse> Handle(ScaleCategoryChangeStateCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _service.ChangeState(request.Id, request.State);
        _logger.LogInformation("Entity with id {RequestId} state change to {RequestState}", request.Id, request.State);
        return new ScaleCategoryChangeStateResponse { Data = result };
    }
}

/// <inheritdoc />
public record ScaleCategoryChangeStateResponse : CQRSResponse<bool>;