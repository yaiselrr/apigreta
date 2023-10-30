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
/// Delete entity
/// </summary>
/// <param name="Id">Entity Id</param>
public record ScaleCategoryDeleteCommand(long Id) : IRequest<ScaleCategoryDeleteResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"delete_scale_category")
    };
}

/// <inheritdoc />
public class ScaleCategoryDeleteHandler : IRequestHandler<ScaleCategoryDeleteCommand, ScaleCategoryDeleteResponse>
{
    private readonly ILogger _logger;
    private readonly IScaleCategoryService _service;


    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    public ScaleCategoryDeleteHandler(
        ILogger<ScaleCategoryDeleteHandler> logger,
        IScaleCategoryService service)
    {
        _logger = logger;
        _service = service;
    }

    /// <inheritdoc />
    public async Task<ScaleCategoryDeleteResponse> Handle(ScaleCategoryDeleteCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _service.Delete(request.Id);
        _logger.LogInformation("Entity with id {RequestId} Deleted successfully", request.Id);
        return new ScaleCategoryDeleteResponse { Data = result };
    }
}

/// <inheritdoc />
public record ScaleCategoryDeleteResponse : CQRSResponse<bool>;