using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Models.Dto.Search;
using Greta.BO.BusinessLogic.Service;
using Greta.BO.BusinessLogic.Specifications.DiscountSpecs;
using Greta.Sdk.Core.Models.Pager;
using MediatR;

namespace Greta.BO.BusinessLogic.Handlers.Queries.Discount;

/// <summary>
/// Query for filter the Discount entities
/// </summary>
/// <param name="CurrentPage">Current page</param>
/// <param name="PageSize">Current size</param>
/// <param name="Filter">Filter object of extend of type <see cref="BaseSearchModel"/></param>
public record DiscountFilterQuery
    (int CurrentPage, int PageSize, DiscountSearchModel Filter) : IRequest<DiscountFilterResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"view_{nameof(Discount).ToLower()}")
    };
}

/// <inheritdoc />
public class DiscountFilterValidator : AbstractValidator<DiscountFilterQuery>
{
    /// <inheritdoc />
    public DiscountFilterValidator()
    {
        RuleFor(x => x.CurrentPage).GreaterThan(0);
        RuleFor(x => x.PageSize).GreaterThan(0);
    }
}

/// <inheritdoc />
public class DiscountFilterHandler : IRequestHandler<DiscountFilterQuery, DiscountFilterResponse>
{
    private readonly IMapper _mapper;
    private readonly IDiscountService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public DiscountFilterHandler(IDiscountService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<DiscountFilterResponse> Handle(DiscountFilterQuery request,
        CancellationToken cancellationToken = default)
    {
        var spec = new DiscountFilterSpec(request.Filter);
        var entities = await _service.FilterSpec(request.CurrentPage, request.PageSize, spec, cancellationToken);
        return new DiscountFilterResponse { Data = _mapper.Map<Pager<DiscountModel>>(entities) };
    }
}

/// <inheritdoc />
public record DiscountFilterResponse : CQRSResponse<Pager<DiscountModel>>;