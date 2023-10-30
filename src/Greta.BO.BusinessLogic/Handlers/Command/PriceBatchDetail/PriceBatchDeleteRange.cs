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

namespace Greta.BO.BusinessLogic.Handlers.Command.PriceBatchDetail;

/// <summary>
/// Delete entities by entity ids
/// </summary>
/// <param name="Ids"></param>
public record PriceBatchDetailDeleteRangeCommand(List<long> Ids) : IRequest<PriceBatchDetailDeleteRangeResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        // new PermissionRequirement.Requirement($"delete_price_batch_detail")
    };
}

/// <inheritdoc />
public class PriceBatchDetailDeleteRangeHandler : IRequestHandler<PriceBatchDetailDeleteRangeCommand, PriceBatchDetailDeleteRangeResponse>
{
    private readonly ILogger _logger;
    private readonly IPriceBatchDetailService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    public PriceBatchDetailDeleteRangeHandler(
        ILogger<PriceBatchDetailDeleteRangeHandler> logger,
        IPriceBatchDetailService service)
    {
        _logger = logger;
        _service = service;
    }

    /// <inheritdoc />
    public async Task<PriceBatchDetailDeleteRangeResponse> Handle(PriceBatchDetailDeleteRangeCommand request,
        CancellationToken cancellationToken)
    {
        var others = request.Ids.Where(x => x != 1).ToList();
        var result = await _service.DeleteRange(others);
        _logger.LogInformation("Entities with ids = {RequestIds} Deleted successfully", request.Ids);
        return new PriceBatchDetailDeleteRangeResponse { Data = result };
    }
}

/// <inheritdoc />
public record PriceBatchDetailDeleteRangeResponse : CQRSResponse<bool>;