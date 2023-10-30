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
using Greta.BO.BusinessLogic.Specifications.OnlineStoreSpecs;
using Greta.Sdk.Core.Models.Pager;
using MediatR;

namespace Greta.BO.BusinessLogic.Handlers.Queries.OnlineStore;

/// <summary>
/// Query for filter the OnlineStore entities
/// </summary>
/// <param name="CurrentPage">Current page</param>
/// <param name="PageSize">Current size</param>
/// <param name="Filter">Filter object of extend of type <see cref="BaseSearchModel"/></param>
public record OnlineStoreFilterQuery
    (int CurrentPage, int PageSize, OnlineStoreSearchModel Filter) : IRequest<OnlineStoreFilterResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"add_edit_{nameof(Store).ToLower()}")
    };
}

/// <inheritdoc />
public class OnlineStoreFilterValidator : AbstractValidator<OnlineStoreFilterQuery>
{
    /// <inheritdoc />
    public OnlineStoreFilterValidator()
    {
        RuleFor(x => x.CurrentPage).GreaterThan(0);
        RuleFor(x => x.PageSize).GreaterThan(0);
    }
}

/// <inheritdoc />
public class OnlineStoreFilterHandler : IRequestHandler<OnlineStoreFilterQuery, OnlineStoreFilterResponse>
{
    private readonly IMapper _mapper;
    private readonly IOnlineStoreService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public OnlineStoreFilterHandler(IOnlineStoreService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<OnlineStoreFilterResponse> Handle(OnlineStoreFilterQuery request,
        CancellationToken cancellationToken = default)
    {
        var spec = new OnlineStoreFilterSpec(request.Filter);
        var entities = await _service.FilterSpec(request.CurrentPage, request.PageSize, spec, cancellationToken);
        return new OnlineStoreFilterResponse { Data = _mapper.Map<Pager<OnlineStoreModel>>(entities) };
    }
}

/// <inheritdoc />
public record OnlineStoreFilterResponse : CQRSResponse<Pager<OnlineStoreModel>>;