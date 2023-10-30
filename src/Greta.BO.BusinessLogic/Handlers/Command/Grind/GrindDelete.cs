using System.Collections.Generic;
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
/// Delete entity by entity id
/// </summary>
/// <param name="Id"></param>
public record GrindDeleteCommand(long Id) : IRequest<GrindDeleteResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"delete_{nameof(Grind).ToLower()}")
    };
}

/// <inheritdoc />
public class GrindDeleteHandler : IRequestHandler<GrindDeleteCommand, GrindDeleteResponse>
{
    private readonly ILogger _logger;
    private readonly IGrindService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    public GrindDeleteHandler(
        ILogger<GrindDeleteHandler> logger,
        IGrindService service)
    {
        _logger = logger;
        _service = service;
    }

    /// <inheritdoc />
    public async Task<GrindDeleteResponse> Handle(GrindDeleteCommand request, CancellationToken cancellationToken)
    {
        var result = await _service.Delete(request.Id);
        _logger.LogInformation("Entity with id {RequestId} Deleted successfully", request.Id);
        return new GrindDeleteResponse { Data = result };
    }
}

/// <inheritdoc />
public record GrindDeleteResponse : CQRSResponse<bool>;