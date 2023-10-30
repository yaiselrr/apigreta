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
/// Delete entity from list if ids
/// </summary>
/// <param name="Ids">List of entity ids</param>
public record ScaleCategoryDeleteRangeCommand(List<long> Ids) : IRequest<ScaleCategoryDeleteRangeResponse>,
    IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"delete_scale_category")
    };
}

/// <inheritdoc />
public class
    ScaleCategoryDeleteRangeHandler : IRequestHandler<ScaleCategoryDeleteRangeCommand, ScaleCategoryDeleteRangeResponse>
{
    private readonly ILogger _logger;
    private readonly IScaleCategoryService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    public ScaleCategoryDeleteRangeHandler(
        ILogger<ScaleCategoryDeleteRangeHandler> logger,
        IScaleCategoryService service)
    {
        _logger = logger;
        _service = service;
    }

    /// <inheritdoc />
    public async Task<ScaleCategoryDeleteRangeResponse> Handle(ScaleCategoryDeleteRangeCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _service.DeleteRange(request.Ids);
        _logger.LogInformation("Entities with ids = {RequestIds} Deleted successfully", request.Ids);
        return new ScaleCategoryDeleteRangeResponse { Data = result };
    }
}

/// <inheritdoc />
public record ScaleCategoryDeleteRangeResponse : CQRSResponse<bool>;