using System.Collections.Generic;
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
/// Delete entity by entity id
/// </summary>
/// <param name="Id"></param>
public record PriceBatchDetailDeleteCommand(long Id) : IRequest<PriceBatchDetailDeleteResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        // new PermissionRequirement.Requirement($"delete_price_batch_detail")
    };
}

/// <inheritdoc />
public class PriceBatchDetailDeleteHandler : IRequestHandler<PriceBatchDetailDeleteCommand, PriceBatchDetailDeleteResponse>
{
    private readonly ILogger _logger;
    private readonly IPriceBatchDetailService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    public PriceBatchDetailDeleteHandler(
        ILogger<PriceBatchDetailDeleteHandler> logger,
        IPriceBatchDetailService service)
    {
        _logger = logger;
        _service = service;
    }

    /// <inheritdoc />
    public async Task<PriceBatchDetailDeleteResponse> Handle(PriceBatchDetailDeleteCommand request, CancellationToken cancellationToken)
    {
        var result = await _service.Delete(request.Id);
        _logger.LogInformation("Entity with id {RequestId} Deleted successfully", request.Id);
        return new PriceBatchDetailDeleteResponse { Data = result };
    }
}

/// <inheritdoc />
public record PriceBatchDetailDeleteResponse : CQRSResponse<bool>;