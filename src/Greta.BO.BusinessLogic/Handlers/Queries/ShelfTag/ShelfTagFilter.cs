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
using Greta.BO.BusinessLogic.Specifications.ShelfTagSpecs;
using Greta.Sdk.Core.Models.Pager;
using MediatR;

namespace Greta.BO.BusinessLogic.Handlers.Queries.ShelfTag;

/// <summary>
/// Query for filter the ShelfTag entities
/// </summary>
/// <param name="CurrentPage">Current page</param>
/// <param name="PageSize">Current size</param>
/// <param name="Filter">Filter object of extend of type <see cref="BaseSearchModel"/></param>
public record ShelfTagFilterQuery
    (int CurrentPage, int PageSize, ShelfTagSearchModel Filter) : IRequest<ShelfTagFilterResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"view_shelf_tags")
    };
}

/// <inheritdoc />
public class ShelfTagFilterValidator : AbstractValidator<ShelfTagFilterQuery>
{
    /// <inheritdoc />
    public ShelfTagFilterValidator()
    {
        RuleFor(x => x.CurrentPage).GreaterThan(0);
        RuleFor(x => x.PageSize).GreaterThan(0);
    }
}

/// <inheritdoc />
public class ShelfTagFilterHandler : IRequestHandler<ShelfTagFilterQuery, ShelfTagFilterResponse>
{
    private readonly IMapper _mapper;
    private readonly IShelfTagService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public ShelfTagFilterHandler(IShelfTagService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<ShelfTagFilterResponse> Handle(ShelfTagFilterQuery request,
        CancellationToken cancellationToken = default)
    {
        var spec = new ShelfTagFilterSpec(request.Filter);
        var entities = await _service.FilterSpec(request.CurrentPage, request.PageSize, spec, cancellationToken);
        return new ShelfTagFilterResponse { Data = _mapper.Map<Pager<ShelfTagModel>>(entities) };
    }
}

/// <inheritdoc />
public record ShelfTagFilterResponse : CQRSResponse<Pager<ShelfTagModel>>;