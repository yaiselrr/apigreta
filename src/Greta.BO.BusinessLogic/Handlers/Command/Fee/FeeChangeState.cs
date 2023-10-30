using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Command.Fee;

/// <summary>
/// Command for change state of Fee
/// </summary>
/// <param name="Id"></param>
/// <param name="State"></param>
public record FeeChangeStateCommand(long Id, bool State) : IRequest<FeeChangeStateResponse>, IAuthorizable
{
    /// <summary>
    /// 
    /// </summary>
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"add_edit_{nameof(Fee).ToLower()}")
    };
}

///<inheritdoc/>
public class FeeChangeStateHandler : IRequestHandler<FeeChangeStateCommand, FeeChangeStateResponse>
{
    private readonly ILogger _logger;
    private readonly IFeeService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    public FeeChangeStateHandler(ILogger<FeeChangeStateHandler> logger, IFeeService service)
    {
        _logger = logger;
        _service = service;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<FeeChangeStateResponse> Handle(FeeChangeStateCommand request, CancellationToken cancellationToken)
    {
        var result = await _service.ChangeState(request.Id, request.State);
        _logger.LogInformation("Entity with id {RequestId} state change to {RequestState}", request.Id, request.State);
        return new FeeChangeStateResponse {Data = result};
    }
}

///<inheritdoc/>
public record FeeChangeStateResponse : CQRSResponse<bool>;
