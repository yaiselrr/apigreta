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

namespace Greta.BO.BusinessLogic.Handlers.Command.Animal;

/// <summary>
/// Delete entities by entity ids
/// </summary>
/// <param name="Ids"></param>
public record AnimalDeleteRangeCommand(List<long> Ids) : IRequest<AnimalDeleteRangeResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"delete_{nameof(Animal).ToLower()}")
    };
}

/// <inheritdoc />
public class AnimalDeleteRangeHandler : IRequestHandler<AnimalDeleteRangeCommand, AnimalDeleteRangeResponse>
{
    private readonly ILogger _logger;
    private readonly IAnimalService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    public AnimalDeleteRangeHandler(
        ILogger<AnimalDeleteRangeHandler> logger,
        IAnimalService service)
    {
        _logger = logger;
        _service = service;
    }

    /// <inheritdoc />
    public async Task<AnimalDeleteRangeResponse> Handle(AnimalDeleteRangeCommand request,
        CancellationToken cancellationToken)
    {
        var others = request.Ids.Where(x => x != 1).ToList();
        var result = await _service.DeleteRange(others);
        _logger.LogInformation("Entities with ids = {RequestIds} Deleted successfully", request.Ids);
        return new AnimalDeleteRangeResponse { Data = result };
    }
}

/// <inheritdoc />
public record AnimalDeleteRangeResponse : CQRSResponse<bool>;