using System.Collections.Generic;
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
/// Delete entity by entity id
/// </summary>
/// <param name="Id"></param>
public record BreedDeleteCommand(long Id) : IRequest<BreedDeleteResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"delete_{nameof(Breed).ToLower()}")
    };
}

/// <inheritdoc />
public class BreedDeleteHandler : IRequestHandler<BreedDeleteCommand, BreedDeleteResponse>
{
    private readonly ILogger _logger;
    private readonly IBreedService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    public BreedDeleteHandler(
        ILogger<BreedDeleteHandler> logger,
        IBreedService service)
    {
        _logger = logger;
        _service = service;
    }

    /// <inheritdoc />
    public async Task<BreedDeleteResponse> Handle(BreedDeleteCommand request, CancellationToken cancellationToken)
    {
        var result = await _service.Delete(request.Id);
        _logger.LogInformation("Entity with id {RequestId} Deleted successfully", request.Id);
        return new BreedDeleteResponse { Data = result };
    }
}

/// <inheritdoc />
public record BreedDeleteResponse : CQRSResponse<bool>;