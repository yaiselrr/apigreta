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
/// Delete entity by entity id
/// </summary>
/// <param name="Id"></param>
public record ExternalScaleDeleteCommand(long Id) : IRequest<ExternalScaleDeleteResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"delete_external_scale")
    };
}

/// <inheritdoc />
public class ExternalScaleDeleteHandler : IRequestHandler<ExternalScaleDeleteCommand, ExternalScaleDeleteResponse>
{
    private readonly ILogger _logger;
    private readonly IExternalScaleService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    public ExternalScaleDeleteHandler(
        ILogger<ExternalScaleDeleteHandler> logger,
        IExternalScaleService service)
    {
        _logger = logger;
        _service = service;
    }

    /// <inheritdoc />
    public async Task<ExternalScaleDeleteResponse> Handle(ExternalScaleDeleteCommand request, CancellationToken cancellationToken)
    {
        var result = await _service.Delete(request.Id);
        _logger.LogInformation("Entity with id {RequestId} Deleted successfully", request.Id);
        return new ExternalScaleDeleteResponse { Data = result };
    }
}

/// <inheritdoc />
public record ExternalScaleDeleteResponse : CQRSResponse<bool>;