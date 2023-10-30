using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Command.Region;

/// <summary>
/// Delete entity by entity id
/// </summary>
/// <param name="Id"></param>
public record RegionDeleteCommand(long Id) : IRequest<RegionDeleteResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"delete_{nameof(Region).ToLower()}")
    };
}

/// <inheritdoc />
public class RegionDeleteHandler : IRequestHandler<RegionDeleteCommand, RegionDeleteResponse>
{
    private readonly ILogger _logger;
    private readonly IRegionService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    public RegionDeleteHandler(
        ILogger<RegionDeleteHandler> logger,
        IRegionService service)
    {
        _logger = logger;
        _service = service;
    }

    /// <inheritdoc />
    public async Task<RegionDeleteResponse> Handle(RegionDeleteCommand request, CancellationToken cancellationToken)
    {
        var result = await _service.Delete(request.Id);
        _logger.LogInformation("Entity with id {RequestId} Deleted successfully", request.Id);
        return new RegionDeleteResponse { Data = result };
    }
}

/// <inheritdoc />
public record RegionDeleteResponse : CQRSResponse<bool>;