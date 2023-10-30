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

namespace Greta.BO.BusinessLogic.Handlers.Command.ShelfTag;

/// <summary>
/// Delete entities by entity ids
/// </summary>
/// <param name="Ids"></param>
public record ShelfTagDeleteRangeCommand(List<long> Ids) : IRequest<ShelfTagDeleteRangeResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"view_shelf_tags")
    };
}

/// <inheritdoc />
public class ShelfTagDeleteRangeHandler : IRequestHandler<ShelfTagDeleteRangeCommand, ShelfTagDeleteRangeResponse>
{
    private readonly ILogger _logger;
    private readonly IShelfTagService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    public ShelfTagDeleteRangeHandler(
        ILogger<ShelfTagDeleteRangeHandler> logger,
        IShelfTagService service)
    {
        _logger = logger;
        _service = service;
    }

    /// <inheritdoc />
    public async Task<ShelfTagDeleteRangeResponse> Handle(ShelfTagDeleteRangeCommand request,
        CancellationToken cancellationToken)
    {
        var others = request.Ids.Where(x => x != 1).ToList();
        var result = await _service.DeleteRange(others);
        _logger.LogInformation("Entities with ids = {RequestIds} Deleted successfully", request.Ids);
        return new ShelfTagDeleteRangeResponse { Data = result };
    }
}

/// <inheritdoc />
public record ShelfTagDeleteRangeResponse : CQRSResponse<bool>;