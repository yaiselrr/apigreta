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
using Greta.BO.BusinessLogic.Specifications.CategorySpecs;
using Greta.Sdk.Core.Models.Pager;
using MediatR;

namespace Greta.BO.BusinessLogic.Handlers.Queries.Category;

/// <summary>
/// Query for filter and paginate the category entities
/// </summary>
/// <param name="CurrentPage">Current page</param>
/// <param name="PageSize">Current size</param>
/// <param name="Filter">Filter object of extend of type <see cref="BaseSearchModel"/></param>
public record CategoryFilterQuery
    (int CurrentPage, int PageSize, CategorySearchModel Filter) : IRequest<CategoryFilterResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"view_{nameof(Category).ToLower()}")
    };
}

/// <inheritdoc />
public class CategoryFilterValidator : AbstractValidator<CategoryFilterQuery>
{
    /// <inheritdoc />
    public CategoryFilterValidator()
    {
        RuleFor(x => x.CurrentPage).GreaterThan(0);
        RuleFor(x => x.PageSize).GreaterThan(0);
    }
}

/// <inheritdoc />
public class CategoryFilterHandler : IRequestHandler<CategoryFilterQuery, CategoryFilterResponse>
{
    private readonly IMapper _mapper;
    private readonly ICategoryService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public CategoryFilterHandler(ICategoryService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<CategoryFilterResponse> Handle(CategoryFilterQuery request,
        CancellationToken cancellationToken = default)
    {
        var spec = new CategoryFilterSpec(request.Filter);
        var entities = await _service.FilterSpec(request.CurrentPage, request.PageSize, spec, cancellationToken);
        return new CategoryFilterResponse { Data = _mapper.Map<Pager<CategoryModel>>(entities) };
    }
}

/// <inheritdoc />
public record CategoryFilterResponse : CQRSResponse<Pager<CategoryModel>>;