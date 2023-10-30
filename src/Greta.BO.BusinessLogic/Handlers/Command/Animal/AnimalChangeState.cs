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
/// Change the state of the entity
/// </summary>
/// <param name="Id">Entity Id</param>
/// <param name="State">State to change</param>
public record AnimalChangeStateCommand(long Id, bool State) : IRequest<AnimalChangeStateResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"add_edit_{nameof(Animal).ToLower()}")
    };
}

/// <inheritdoc />
public class AnimalChangeStateHandler : IRequestHandler<AnimalChangeStateCommand, AnimalChangeStateResponse>
{
    private readonly ILogger _logger;
    private readonly IAnimalService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    public AnimalChangeStateHandler(ILogger<AnimalChangeStateHandler> logger, IAnimalService service)
    {
        _logger = logger;
        _service = service;
    }

    /// <inheritdoc />
    public async Task<AnimalChangeStateResponse> Handle(AnimalChangeStateCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _service.ChangeState(request.Id, request.State);
        _logger.LogInformation("Entity with id {RequestId} state change to {RequestState}", request.Id, request.State);
        return new AnimalChangeStateResponse { Data = result };
    }
}

/// <inheritdoc />
public record AnimalChangeStateResponse : CQRSResponse<bool>;