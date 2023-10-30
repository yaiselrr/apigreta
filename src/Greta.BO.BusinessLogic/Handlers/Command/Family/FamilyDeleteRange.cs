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

namespace Greta.BO.BusinessLogic.Handlers.Command.Family;

/// <summary>
/// Delete entities by entity ids
/// </summary>
/// <param name="Ids"></param>
public record FamilyDeleteRangeCommand(List<long> Ids) : IRequest<FamilyDeleteRangeResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"delete_{nameof(Family).ToLower()}")
    };
}

/// <inheritdoc />
public class FamilyDeleteRangeHandler : IRequestHandler<FamilyDeleteRangeCommand, FamilyDeleteRangeResponse>
{
    private readonly ILogger _logger;
    private readonly IFamilyService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    public FamilyDeleteRangeHandler(
        ILogger<FamilyDeleteRangeHandler> logger,
        IFamilyService service)
    {
        _logger = logger;
        _service = service;
    }

    /// <inheritdoc />
    public async Task<FamilyDeleteRangeResponse> Handle(FamilyDeleteRangeCommand request,
        CancellationToken cancellationToken)
    {
        var idScanRemove = new List<long>();
        foreach (var id in request.Ids)
        {
            if (!await _service.CanDeleted(id))
            {
                idScanRemove.Add(id);
            }
        }

        var result = await _service.DeleteRange(request.Ids.Except(idScanRemove).ToList());
        _logger.LogInformation("Entities with ids = {RequestIds} Deleted successfully", request.Ids);
        return new FamilyDeleteRangeResponse { Data = result };
    }
}

/// <inheritdoc />
public record FamilyDeleteRangeResponse : CQRSResponse<bool>;