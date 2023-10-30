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
using Greta.BO.BusinessLogic.Specifications.FamilySpecs;
using Greta.Sdk.Core.Models.Pager;
using MediatR;

namespace Greta.BO.BusinessLogic.Handlers.Queries.Family;

/// <summary>
/// Query for filter the Tax entities
/// </summary>
/// <param name="CurrentPage">Current page</param>
/// <param name="PageSize">Current size</param>
/// <param name="Filter">Filter object of extend of type <see cref="BaseSearchModel"/></param>
public record FamilyProductsFilterQuery
    (int CurrentPage, int PageSize, FamilySearchModel Filter) : IRequest<FamilyProductsResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"view_{nameof(Family).ToLower()}")
    };
}

/// <inheritdoc />
public class FamilyProductsValidator : AbstractValidator<FamilyProductsFilterQuery>
{
    /// <inheritdoc />
    public FamilyProductsValidator()
    {
        RuleFor(x => x.CurrentPage).GreaterThan(0);
        RuleFor(x => x.PageSize).GreaterThan(0);
    }
}

/// <inheritdoc />
public class FamilyProductsHandler : IRequestHandler<FamilyProductsFilterQuery, FamilyProductsResponse>
{
    private readonly IMapper _mapper;
    private readonly IFamilyService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public FamilyProductsHandler(IFamilyService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<FamilyProductsResponse> Handle(FamilyProductsFilterQuery request, CancellationToken cancellationToken)
    {
        var spec = new ProductByFamilyIdSpec(request.Filter.Search, request.Filter.Sort, request.Filter.FamilyId);

        var entities = await _service.FilterFamily(
            request.CurrentPage,
            request.PageSize,
            spec
        );

        return new FamilyProductsResponse() { Data = this._mapper.Map<Pager<ProductModel>>(entities) };
    }
}

/// <inheritdoc />
public record FamilyProductsResponse : CQRSResponse<Pager<ProductModel>>;