using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;

namespace Greta.BO.BusinessLogic.Handlers.Queries.LoyaltyDiscount;

/// <summary>
/// Query for get list of Loyalty Discount Remain Stores
/// </summary>
public record LoyaltyDiscountRemainStoresQuery : IRequest<LoyaltyDiscountRemainStoresResponse>, IAuthorizable
{
    /// <summary>
    /// 
    /// </summary>
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"view_loyalty_discount")
    };
}

///<inheritdoc/>
public class LoyaltyDiscountRemainStoresHandler : IRequestHandler<LoyaltyDiscountRemainStoresQuery, LoyaltyDiscountRemainStoresResponse>
{
    private readonly IMapper _mapper;
    private readonly ILoyaltyDiscountService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public LoyaltyDiscountRemainStoresHandler(ILoyaltyDiscountService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<LoyaltyDiscountRemainStoresResponse> Handle(LoyaltyDiscountRemainStoresQuery request, CancellationToken cancellationToken)
    {
        var entities = await _service.GetRemainStores();
        return new LoyaltyDiscountRemainStoresResponse {Data = _mapper.Map<List<StoreModel>>(entities)};
    }
}

///<inheritdoc/>
public record LoyaltyDiscountRemainStoresResponse : CQRSResponse<List<StoreModel>>;
  
