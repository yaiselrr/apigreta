using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Models.Dto.LoyaltyDiscountDto;
using Greta.BO.BusinessLogic.Service;
using MediatR;

namespace Greta.BO.BusinessLogic.Handlers.Queries.LoyaltyDiscount;

/// <summary>
/// Query for get all LoyaltyDiscount
/// </summary>
public record LoyaltyDiscountGetAllQuery : IRequest<LoyaltyDiscountGetAllResponse>, IAuthorizable
{
    /// <summary>
    /// 
    /// </summary>
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"view_loyalty_discount")
    };
}

/// <summary>
/// <inheritdoc/>
/// </summary>
public class LoyaltyDiscountGetAllHandler : IRequestHandler<LoyaltyDiscountGetAllQuery, LoyaltyDiscountGetAllResponse>
{
    private readonly IMapper _mapper;
    private readonly ILoyaltyDiscountService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public LoyaltyDiscountGetAllHandler(ILoyaltyDiscountService service, IMapper mapper)
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
    public async Task<LoyaltyDiscountGetAllResponse> Handle(LoyaltyDiscountGetAllQuery request, CancellationToken cancellationToken=default)
    {
        var entities = await _service.Get();
        return new LoyaltyDiscountGetAllResponse { Data = _mapper.Map<List<LoyaltyDiscountGetAllModel>>(entities)};
    }
}
///<inheritdoc/>
public record LoyaltyDiscountGetAllResponse : CQRSResponse<List<LoyaltyDiscountGetAllModel>>;
