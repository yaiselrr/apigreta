using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Command.LoyaltyDiscount;

/// <summary>
/// Query for change state of LoyaltyDiscount
/// </summary>
/// <param name="Id"></param>
/// <param name="State"></param>
public record LoyaltyDiscountChangeStateCommand(long Id, bool State) : IRequest<LoyaltyDiscountChangeStateResponse>, IAuthorizable
{
    /// <summary>
    /// 
    /// </summary>
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"add_edit_loyalty_discount")
    };
}
///<inheritdoc/>
public class LoyaltyDiscountChangeStateHandler : IRequestHandler<LoyaltyDiscountChangeStateCommand, LoyaltyDiscountChangeStateResponse>
{
    private readonly ILogger _logger;
    private readonly ILoyaltyDiscountService _service;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    public LoyaltyDiscountChangeStateHandler(ILogger<LoyaltyDiscountChangeStateHandler> logger, ILoyaltyDiscountService service)
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
    public async Task<LoyaltyDiscountChangeStateResponse> Handle(LoyaltyDiscountChangeStateCommand request, CancellationToken cancellationToken=default)
    {
        var result = await _service.ChangeState(request.Id, request.State);
        _logger.LogInformation("Entity with id {RequestId} state change to {RequestState}", request.Id, request.State);
        return new LoyaltyDiscountChangeStateResponse { Data = result};
    }
}
///<inheritdoc/>
public record LoyaltyDiscountChangeStateResponse : CQRSResponse<bool>;
