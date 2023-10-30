using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Command.Region;

/// <summary>
/// Delete entities by entity ids
/// </summary>
/// <param name="Ids"></param>
public record RegionDeleteRangeCommand(List<long> Ids) : IRequest<RegionDeleteRangeResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"delete_{nameof(Region).ToLower()}")
    };
}

/// <inheritdoc />
public class RegionDeleteRangeHandler : IRequestHandler<RegionDeleteRangeCommand, RegionDeleteRangeResponse>
{
    private readonly ILogger _logger;
    private readonly IRegionService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    public RegionDeleteRangeHandler(
        ILogger<RegionDeleteRangeHandler> logger,
        IRegionService service)
    {
        _logger = logger;
        _service = service;
    }

    /// <inheritdoc />
    public async Task<RegionDeleteRangeResponse> Handle(RegionDeleteRangeCommand request,
        CancellationToken cancellationToken)
    {
        var others = request.Ids.Where(x => x != 1).ToList();
        var result = await _service.DeleteRange(others);
        _logger.LogInformation("Entities with ids = {RequestIds} Deleted successfully", request.Ids);
        return new RegionDeleteRangeResponse { Data = result };
    }
}

/// <inheritdoc />
public record RegionDeleteRangeResponse : CQRSResponse<bool>;