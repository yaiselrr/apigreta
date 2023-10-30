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
/// Query for delete range of LoyaltyDiscount
/// </summary>
/// <param name="Ids"></param>
public record LoyaltyDiscountDeleteRangeCommand(List<long> Ids) : IRequest<LoyaltyDiscountDeleteRangeResponse>, IAuthorizable
{
    /// <summary>
    /// 
    /// </summary>
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"delete_loyalty_discount")
    };
}

///<inheritdoc/>
public class LoyaltyDiscountDeleteRangeHandler : IRequestHandler<LoyaltyDiscountDeleteRangeCommand, LoyaltyDiscountDeleteRangeResponse>
{
    private readonly ILogger _logger;
    private readonly ILoyaltyDiscountService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    public LoyaltyDiscountDeleteRangeHandler(ILogger<LoyaltyDiscountDeleteRangeHandler> logger, ILoyaltyDiscountService service)
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
    public async Task<LoyaltyDiscountDeleteRangeResponse> Handle(LoyaltyDiscountDeleteRangeCommand request, CancellationToken cancellationToken=default)
    {
        var result = await _service.DeleteRange(request.Ids);
        _logger.LogInformation("Entities with ids = {RequestIds} Deleted successfully", request.Ids);
        return new LoyaltyDiscountDeleteRangeResponse { Data = result};
    }
}

///<inheritdoc/>
public record LoyaltyDiscountDeleteRangeResponse : CQRSResponse<bool>;
