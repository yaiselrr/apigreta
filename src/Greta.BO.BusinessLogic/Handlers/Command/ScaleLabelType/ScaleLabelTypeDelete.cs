using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Command.ScaleLabelType;

/// <summary>
/// Delete entity by entity id
/// </summary>
/// <param name="Id"></param>
public record ScaleLabelTypeDeleteCommand(long Id) : IRequest<ScaleLabelTypeDeleteResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"delete_scale_label_type")
    };
}

/// <inheritdoc />
public class ScaleLabelTypeDeleteHandler : IRequestHandler<ScaleLabelTypeDeleteCommand, ScaleLabelTypeDeleteResponse>
{
    private readonly ILogger _logger;
    private readonly IScaleLabelTypeService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    public ScaleLabelTypeDeleteHandler(
        ILogger<ScaleLabelTypeDeleteHandler> logger,
        IScaleLabelTypeService service)
    {
        _logger = logger;
        _service = service;
    }

    /// <inheritdoc />
    public async Task<ScaleLabelTypeDeleteResponse> Handle(ScaleLabelTypeDeleteCommand request, CancellationToken cancellationToken)
    {
        var result = await _service.Delete(request.Id);
        _logger.LogInformation($"Entity with id {request.Id} Deleted successfully.");
        return new ScaleLabelTypeDeleteResponse { Data = result };
    }
}

/// <inheritdoc />
public record ScaleLabelTypeDeleteResponse : CQRSResponse<bool>;