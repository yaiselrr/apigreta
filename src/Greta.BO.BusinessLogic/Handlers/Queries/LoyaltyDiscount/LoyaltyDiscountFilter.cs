using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Models.Dto.LoyaltyDiscountDto;
using Greta.BO.BusinessLogic.Models.Dto.Search;
using Greta.BO.BusinessLogic.Service;
using Greta.BO.BusinessLogic.Specifications.LoyaltyDiscountSpecs;
using Greta.Sdk.Core.Models.Pager;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Queries.LoyaltyDiscount;

/// <summary>
/// Query for filter and paginate LoyaltyDiscount
/// </summary>
/// <param name="CurrentPage"></param>
/// <param name="PageSize"></param>
/// <param name="Filter"></param>
public record LoyaltyDiscountFilterQuery(int CurrentPage, int PageSize, LoyaltyDiscountSearchModel Filter) : IRequest<LoyaltyDiscountFilterResponse>, IAuthorizable
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
public class LoyaltyDiscountFilterValidator : AbstractValidator<LoyaltyDiscountFilterQuery>
{
    /// <summary>
    /// 
    /// </summary>
    public LoyaltyDiscountFilterValidator()
    {
        RuleFor(x => x.CurrentPage).GreaterThan(0);
        RuleFor(x => x.PageSize).GreaterThan(0);
    }
}

///<inheritdoc/>
public class LoyaltyDiscountFilterHandler : IRequestHandler<LoyaltyDiscountFilterQuery, LoyaltyDiscountFilterResponse>
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly ILoyaltyDiscountService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public LoyaltyDiscountFilterHandler(ILogger<LoyaltyDiscountFilterHandler> logger, ILoyaltyDiscountService service, IMapper mapper)
    {
        _logger = logger;
        _service = service;
        _mapper = mapper;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="BusinessLogicException"></exception>
    public async Task<LoyaltyDiscountFilterResponse> Handle(LoyaltyDiscountFilterQuery request, CancellationToken cancellationToken = default)
    {
        if (request.CurrentPage < 1 || request.PageSize < 1)
        {
            _logger.LogError("Page parameter (currentPage or pageSize) out of bounds");
            throw new BusinessLogicException("Page parameter out of bounds");
        }

        var spec = new LoyaltyDiscountFilterSpec(request.Filter);
        var entities = await _service.FilterSpec(request.CurrentPage, request.PageSize, spec);
        return new LoyaltyDiscountFilterResponse { Data = _mapper.Map<Pager<LoyaltyDiscountFilterModel>>(entities)};
    }
}

///<inheritdoc/>
public record LoyaltyDiscountFilterResponse : CQRSResponse<Pager<LoyaltyDiscountFilterModel>>;
