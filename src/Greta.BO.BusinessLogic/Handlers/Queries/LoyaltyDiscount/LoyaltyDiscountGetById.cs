using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Core.Caching;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Models.Dto.LoyaltyDiscountDto;
using Greta.BO.BusinessLogic.Service;
using Greta.BO.BusinessLogic.Specifications.Generics;
using MediatR;

namespace Greta.BO.BusinessLogic.Handlers.Queries.LoyaltyDiscount;

/// <summary>
/// Query for get by id LoyaltyDiscount
/// </summary>
/// <param name="Id"></param>
public record LoyaltyDiscountGetByIdQuery(long Id) : IRequest<LoyaltyDiscountGetByIdResponse>, IAuthorizable, ICacheable
{
    /// <summary>
    /// 
    /// </summary>
    public List<IRequirement> Requirements => new() {
        new PermissionRequirement.Requirement($"view_loyalty_discount")
    };

    /// <inheritdoc/>
    public string CacheKey => $"LoyaltyDiscountGetById{Id}";
}

///<inheritdoc/>
public class LoyaltyDiscountGetByIdHandler : IRequestHandler<LoyaltyDiscountGetByIdQuery, LoyaltyDiscountGetByIdResponse>
{
    private readonly IMapper _mapper;
    private readonly ILoyaltyDiscountService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public LoyaltyDiscountGetByIdHandler(ILoyaltyDiscountService service, IMapper mapper)
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
    public async Task<LoyaltyDiscountGetByIdResponse> Handle(LoyaltyDiscountGetByIdQuery request, CancellationToken cancellationToken=default)
    {
        var entity = await _service.Get(new GetByIdSpec<Api.Entities.LoyaltyDiscount>(request.Id), cancellationToken);
        var data = _mapper.Map<LoyaltyDiscountGetByIdModel>(entity);
        return data == null ? null : new LoyaltyDiscountGetByIdResponse {Data = data};
    }
}

///<inheritdoc/>
public record LoyaltyDiscountGetByIdResponse : CQRSResponse<LoyaltyDiscountGetByIdModel>;
