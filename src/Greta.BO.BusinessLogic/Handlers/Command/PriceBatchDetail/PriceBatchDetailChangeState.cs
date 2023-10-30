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
/// Change the state of the entity
/// </summary>
/// <param name="Id">Entity Id</param>
/// <param name="State">State to change</param>
public record PriceBatchDetailChangeStateCommand(long Id, bool State) : IRequest<PriceBatchDetailChangeStateResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        // new PermissionRequirement.Requirement($"add_edit_price_batch")
    };
}

/// <inheritdoc />
public class PriceBatchDetailChangeStateHandler : IRequestHandler<PriceBatchDetailChangeStateCommand, PriceBatchDetailChangeStateResponse>
{
    private readonly ILogger _logger;
    private readonly IPriceBatchDetailService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    public PriceBatchDetailChangeStateHandler(ILogger<PriceBatchDetailChangeStateHandler> logger, IPriceBatchDetailService service)
    {
        _logger = logger;
        _service = service;
    }

    /// <inheritdoc />
    public async Task<PriceBatchDetailChangeStateResponse> Handle(PriceBatchDetailChangeStateCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _service.ChangeState(request.Id, request.State);
        _logger.LogInformation("Entity with id {RequestId} state change to {RequestState}", request.Id, request.State);
        return new PriceBatchDetailChangeStateResponse { Data = result };
    }
}

/// <inheritdoc />
public record PriceBatchDetailChangeStateResponse : CQRSResponse<bool>;