using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Models.Dto.Search;
using Greta.BO.BusinessLogic.Service;
using Greta.BO.BusinessLogic.Specifications.StoreSpecs;
using Greta.Sdk.Core.Models.Pager;
using MediatR;

namespace Greta.BO.BusinessLogic.Handlers.Queries.Store;

/// <summary>
/// Query for filter the Store entities
/// </summary>
/// <param name="CurrentPage">Current page</param>
/// <param name="PageSize">Current size</param>
/// <param name="Filter">Filter object of extend of type <see cref="BaseSearchModel"/></param>
public record StoreFilterQuery
    (int CurrentPage, int PageSize, StoreSearchModel Filter) : IRequest<StoreFilterResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"view_{nameof(Store).ToLower()}")
    };
}

/// <inheritdoc />
public class StoreFilterValidator : AbstractValidator<StoreFilterQuery>
{
    /// <inheritdoc />
    public StoreFilterValidator()
    {
        RuleFor(x => x.CurrentPage).GreaterThan(0);
        RuleFor(x => x.PageSize).GreaterThan(0);
    }
}

/// <inheritdoc />
public class StoreFilterHandler : IRequestHandler<StoreFilterQuery, StoreFilterResponse>
{
    private readonly IStoreService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    public StoreFilterHandler(IStoreService service)
    {
        _service = service;
    }

    /// <inheritdoc />
    public async Task<StoreFilterResponse> Handle(StoreFilterQuery request,
        CancellationToken cancellationToken = default)
    {
        var spec = new StoreFilterSpec(request.Filter);
        var entities = await _service.FilterSpec(request.CurrentPage, request.PageSize, spec, cancellationToken);
        return new StoreFilterResponse { Data = entities };
    }
}

/// <inheritdoc />
public record StoreFilterResponse : CQRSResponse<Pager<StoreListModel>>;