using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Command.ShelfTag;

/// <summary>
/// Update entity
/// </summary>
/// <param name="Id">Entity id</param>
/// <param name="ValueQty">Value Qty</param>
public record ShelfTagUpdateCommand
    (long Id, int ValueQty) : IRequest<ShelfTagUpdateResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"view_shelf_tags")
    };
}

/// <inheritdoc />
public class ShelfTagUpdateHandler : IRequestHandler<ShelfTagUpdateCommand, ShelfTagUpdateResponse>
{
    private readonly ILogger _logger;
    private readonly IShelfTagService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    public ShelfTagUpdateHandler(
        ILogger<ShelfTagUpdateHandler> logger,
        IShelfTagService service)
    {
        _logger = logger;
        _service = service;
    }

    /// <inheritdoc />
    public async Task<ShelfTagUpdateResponse> Handle(ShelfTagUpdateCommand request, CancellationToken cancellationToken)
    {

        var success = await _service.PutByQty(request.Id, request.ValueQty);
        _logger.LogInformation("ShelfTag {ShelfTagId} update successfully", request.Id);
        return new ShelfTagUpdateResponse { Data = success };
    }
}

/// <inheritdoc />
public record ShelfTagUpdateResponse : CQRSResponse<bool>;