using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Command.Grind;

/// <summary>
/// Delete entities by entity ids
/// </summary>
/// <param name="Ids"></param>
public record GrindDeleteRangeCommand(List<long> Ids) : IRequest<GrindDeleteRangeResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"delete_{nameof(Grind).ToLower()}")
    };
}

/// <inheritdoc />
public class GrindDeleteRangeHandler : IRequestHandler<GrindDeleteRangeCommand, GrindDeleteRangeResponse>
{
    private readonly ILogger _logger;
    private readonly IGrindService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    public GrindDeleteRangeHandler(
        ILogger<GrindDeleteRangeHandler> logger,
        IGrindService service)
    {
        _logger = logger;
        _service = service;
    }

    /// <inheritdoc />
    public async Task<GrindDeleteRangeResponse> Handle(GrindDeleteRangeCommand request,
        CancellationToken cancellationToken)
    {
        var others = request.Ids.Where(x => x != 1).ToList();
        var result = await _service.DeleteRange(others);
        _logger.LogInformation("Entities with ids = {RequestIds} Deleted successfully", request.Ids);
        return new GrindDeleteRangeResponse { Data = result };
    }
}

/// <inheritdoc />
public record GrindDeleteRangeResponse : CQRSResponse<bool>;