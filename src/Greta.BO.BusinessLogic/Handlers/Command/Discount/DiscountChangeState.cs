using System.Collections.Generic;
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
/// Change the state of the entity
/// </summary>
/// <param name="Id">Entity Id</param>
/// <param name="State">State to change</param>
public record DiscountChangeStateCommand(long Id, bool State) : IRequest<DiscountChangeStateResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"add_edit_{nameof(Discount).ToLower()}")
    };
}

/// <inheritdoc />
public class DiscountChangeStateHandler : IRequestHandler<DiscountChangeStateCommand, DiscountChangeStateResponse>
{
    private readonly ILogger _logger;
    private readonly IDiscountService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    public DiscountChangeStateHandler(ILogger<DiscountChangeStateHandler> logger, IDiscountService service)
    {
        _logger = logger;
        _service = service;
    }

    /// <inheritdoc />
    public async Task<DiscountChangeStateResponse> Handle(DiscountChangeStateCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _service.ChangeState(request.Id, request.State);
        _logger.LogInformation("Entity with id {RequestId} state change to {RequestState}", request.Id, request.State);
        return new DiscountChangeStateResponse { Data = result };
    }
}

/// <inheritdoc />
public record DiscountChangeStateResponse : CQRSResponse<bool>;