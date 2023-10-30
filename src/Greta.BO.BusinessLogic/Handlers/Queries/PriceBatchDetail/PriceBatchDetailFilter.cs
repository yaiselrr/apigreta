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
using Greta.BO.BusinessLogic.Specifications.PriceBatchDetailSpecs;
using Greta.Sdk.Core.Models.Pager;
using MediatR;

namespace Greta.BO.BusinessLogic.Handlers.Queries.PriceBatchDetail;

/// <summary>
/// Query for filter the PriceBatchDetail entities
/// </summary>
/// <param name="CurrentPage">Current page</param>
/// <param name="PageSize">Current size</param>
/// <param name="Filter">Filter object of extend of type <see cref="BaseSearchModel"/></param>
public record PriceBatchDetailFilterQuery
    (int CurrentPage, int PageSize, PriceBatchDetailSearchModel Filter) : IRequest<PriceBatchDetailFilterResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"view_price_batch")
    };
}

/// <inheritdoc />
public class PriceBatchDetailFilterValidator : AbstractValidator<PriceBatchDetailFilterQuery>
{
    /// <inheritdoc />
    public PriceBatchDetailFilterValidator()
    {
        RuleFor(x => x.CurrentPage).GreaterThan(0);
        RuleFor(x => x.PageSize).GreaterThan(0);
    }
}

/// <inheritdoc />
public class PriceBatchDetailFilterHandler : IRequestHandler<PriceBatchDetailFilterQuery, PriceBatchDetailFilterResponse>
{
    private readonly IMapper _mapper;
    private readonly IPriceBatchDetailService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public PriceBatchDetailFilterHandler(IPriceBatchDetailService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<PriceBatchDetailFilterResponse> Handle(PriceBatchDetailFilterQuery request,
        CancellationToken cancellationToken = default)
    {
        var spec = new PriceBatchDetailFilterSpec(request.Filter);
        var entities = await _service.FilterSpec(request.CurrentPage, request.PageSize, spec, cancellationToken);
        return new PriceBatchDetailFilterResponse { Data = _mapper.Map<Pager<PriceBatchDetailModel>>(entities) };
    }
}

/// <inheritdoc />
public record PriceBatchDetailFilterResponse : CQRSResponse<Pager<PriceBatchDetailModel>>;