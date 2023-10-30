using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Command.Animal;

/// <summary>
/// Delete entity by entity id
/// </summary>
/// <param name="Id"></param>
public record AnimalDeleteCommand(long Id) : IRequest<AnimalDeleteResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"delete_{nameof(Animal).ToLower()}")
    };
}

/// <inheritdoc />
public class AnimalDeleteHandler : IRequestHandler<AnimalDeleteCommand, AnimalDeleteResponse>
{
    private readonly ILogger _logger;
    private readonly IAnimalService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    public AnimalDeleteHandler(
        ILogger<AnimalDeleteHandler> logger,
        IAnimalService service)
    {
        _logger = logger;
        _service = service;
    }

    /// <inheritdoc />
    public async Task<AnimalDeleteResponse> Handle(AnimalDeleteCommand request, CancellationToken cancellationToken)
    {
        var result = await _service.Delete(request.Id);
        _logger.LogInformation("Entity with id {RequestId} Deleted successfully", request.Id);
        return new AnimalDeleteResponse { Data = result };
    }
}

/// <inheritdoc />
public record AnimalDeleteResponse : CQRSResponse<bool>;