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
using FluentValidation;

namespace Greta.BO.BusinessLogic.Handlers.Command.OnlineStore;

/// <summary>
/// Delete entities by entity ids
/// </summary>
/// <param name="Ids"></param>
public record OnlineStoreDeleteRangeCommand(List<long> Ids) : IRequest<OnlineStoreDeleteRangeResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"add_edit_{nameof(Store).ToLower()}")
    };
}

public class Validator : AbstractValidator<OnlineStoreDeleteRangeCommand>
{
    private readonly IOnlineStoreService _service;

    public Validator(IOnlineStoreService service)
    {
        _service = service;

        RuleFor(x => x.Ids)
            .MustAsync(CanDeleted)
            .WithMessage($"Some online stores cannot be deleted because it is associated with another element.");
    }

    private async Task<bool> CanDeleted(List<long> ids, CancellationToken cancellationToken)
    {
        return await _service.CanDeleted(ids);
    }
}

/// <inheritdoc />
public class OnlineStoreDeleteRangeHandler : IRequestHandler<OnlineStoreDeleteRangeCommand, OnlineStoreDeleteRangeResponse>
{
    private readonly ILogger _logger;
    private readonly IOnlineStoreService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    public OnlineStoreDeleteRangeHandler(
        ILogger<OnlineStoreDeleteRangeHandler> logger,
        IOnlineStoreService service)
    {
        _logger = logger;
        _service = service;
    }

    /// <inheritdoc />
    public async Task<OnlineStoreDeleteRangeResponse> Handle(OnlineStoreDeleteRangeCommand request,
        CancellationToken cancellationToken)
    {
        var others = request.Ids.Where(x => x != 1).ToList();
        var result = await _service.DeleteRange(others);
        _logger.LogInformation("Entities with ids = {RequestIds} Deleted successfully", request.Ids);
        return new OnlineStoreDeleteRangeResponse { Data = result };
    }
}

/// <inheritdoc />
public record OnlineStoreDeleteRangeResponse : CQRSResponse<bool>;