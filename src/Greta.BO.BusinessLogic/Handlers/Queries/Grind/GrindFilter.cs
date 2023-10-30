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
using Greta.BO.BusinessLogic.Specifications.GrindSpecs;
using Greta.Sdk.Core.Models.Pager;
using MediatR;

namespace Greta.BO.BusinessLogic.Handlers.Queries.Grind;

/// <summary>
/// Query for filter the Grind entities
/// </summary>
/// <param name="CurrentPage">Current page</param>
/// <param name="PageSize">Current size</param>
/// <param name="Filter">Filter object of extend of type <see cref="BaseSearchModel"/></param>
public record GrindFilterQuery
    (int CurrentPage, int PageSize, GrindSearchModel Filter) : IRequest<GrindFilterResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"view_{nameof(Grind).ToLower()}")
    };
}

/// <inheritdoc />
public class GrindFilterValidator : AbstractValidator<GrindFilterQuery>
{
    /// <inheritdoc />
    public GrindFilterValidator()
    {
        RuleFor(x => x.CurrentPage).GreaterThan(0);
        RuleFor(x => x.PageSize).GreaterThan(0);
    }
}

/// <inheritdoc />
public class GrindFilterHandler : IRequestHandler<GrindFilterQuery, GrindFilterResponse>
{
    private readonly IMapper _mapper;
    private readonly IGrindService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public GrindFilterHandler(IGrindService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<GrindFilterResponse> Handle(GrindFilterQuery request,
        CancellationToken cancellationToken = default)
    {
        var spec = new GrindFilterSpec(request.Filter);
        var entities = await _service.FilterSpec(request.CurrentPage, request.PageSize, spec, cancellationToken);
        return new GrindFilterResponse { Data = _mapper.Map<Pager<GrindModel>>(entities) };
    }
}

/// <inheritdoc />
public record GrindFilterResponse : CQRSResponse<Pager<GrindModel>>;