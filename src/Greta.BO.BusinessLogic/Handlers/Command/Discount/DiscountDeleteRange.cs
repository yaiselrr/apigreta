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

namespace Greta.BO.BusinessLogic.Handlers.Command.Discount;

/// <summary>
/// Delete entities by entity ids
/// </summary>
/// <param name="Ids"></param>
public record DiscountDeleteRangeCommand(List<long> Ids) : IRequest<DiscountDeleteRangeResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"delete_{nameof(Discount).ToLower()}")
    };
}

/// <inheritdoc />
public class DiscountDeleteRangeHandler : IRequestHandler<DiscountDeleteRangeCommand, DiscountDeleteRangeResponse>
{
    private readonly ILogger _logger;
    private readonly IDiscountService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    public DiscountDeleteRangeHandler(
        ILogger<DiscountDeleteRangeHandler> logger,
        IDiscountService service)
    {
        _logger = logger;
        _service = service;
    }

    /// <inheritdoc />
    public async Task<DiscountDeleteRangeResponse> Handle(DiscountDeleteRangeCommand request,
        CancellationToken cancellationToken)
    {
        var others = request.Ids.Where(x => x != 1).ToList();
        var result = await _service.DeleteRange(others);
        _logger.LogInformation("Entities with ids = {RequestIds} Deleted successfully", request.Ids);
        return new DiscountDeleteRangeResponse { Data = result };
    }
}

/// <inheritdoc />
public record DiscountDeleteRangeResponse : CQRSResponse<bool>;