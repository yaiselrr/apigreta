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
/// Delete entities by entity ids
/// </summary>
/// <param name="Ids"></param>
public record ExternalScaleDeleteRangeCommand(List<long> Ids) : IRequest<ExternalScaleDeleteRangeResponse>,
    IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"delete_external_scale")
    };
}

/// <inheritdoc />
public class
    ExternalScaleDeleteRangeHandler : IRequestHandler<ExternalScaleDeleteRangeCommand, ExternalScaleDeleteRangeResponse>
{
    private readonly ILogger _logger;
    private readonly IExternalScaleService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    public ExternalScaleDeleteRangeHandler(
        ILogger<ExternalScaleDeleteRangeHandler> logger,
        IExternalScaleService service)
    {
        _logger = logger;
        _service = service;
    }

    /// <inheritdoc />
    public async Task<ExternalScaleDeleteRangeResponse> Handle(ExternalScaleDeleteRangeCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _service.DeleteRange(request.Ids);
        _logger.LogInformation("Entities with ids = {RequestIds} Deleted successfully", request.Ids);
        return new ExternalScaleDeleteRangeResponse { Data = result };
    }
}

/// <inheritdoc />
public record ExternalScaleDeleteRangeResponse : CQRSResponse<bool>;