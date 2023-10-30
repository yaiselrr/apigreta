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

namespace Greta.BO.BusinessLogic.Handlers.Command.Breed;

/// <summary>
/// Delete entities by entity ids
/// </summary>
/// <param name="Ids"></param>
public record BreedDeleteRangeCommand(List<long> Ids) : IRequest<BreedDeleteRangeResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"delete_{nameof(Breed).ToLower()}")
    };
}

/// <inheritdoc />
public class BreedDeleteRangeHandler : IRequestHandler<BreedDeleteRangeCommand, BreedDeleteRangeResponse>
{
    private readonly ILogger _logger;
    private readonly IBreedService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    public BreedDeleteRangeHandler(
        ILogger<BreedDeleteRangeHandler> logger,
        IBreedService service)
    {
        _logger = logger;
        _service = service;
    }

    /// <inheritdoc />
    public async Task<BreedDeleteRangeResponse> Handle(BreedDeleteRangeCommand request,
        CancellationToken cancellationToken)
    {
        var others = request.Ids.Where(x => x != 1).ToList();
        var result = await _service.DeleteRange(others);
        _logger.LogInformation("Entities with ids = {RequestIds} Deleted successfully", request.Ids);
        return new BreedDeleteRangeResponse { Data = result };
    }
}

/// <inheritdoc />
public record BreedDeleteRangeResponse : CQRSResponse<bool>;