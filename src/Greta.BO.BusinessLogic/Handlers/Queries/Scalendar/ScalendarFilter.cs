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
using Greta.BO.BusinessLogic.Specifications.ScalendarSpecs;
using Greta.Sdk.Core.Models.Pager;
using MediatR;

namespace Greta.BO.BusinessLogic.Handlers.Queries.Scalendar;

/// <summary>
/// Query for filter the Scalendar entities
/// </summary>
/// <param name="CurrentPage">Current page</param>
/// <param name="PageSize">Current size</param>
/// <param name="Filter">Filter object of extend of type <see cref="BaseSearchModel"/></param>
public record ScalendarFilterQuery
    (int CurrentPage, int PageSize, ScalendarSearchModel Filter) : IRequest<ScalendarFilterResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"view_{nameof(Scalendar).ToLower()}")
    };
}

/// <inheritdoc />
public class ScalendarFilterValidator : AbstractValidator<ScalendarFilterQuery>
{
    /// <inheritdoc />
    public ScalendarFilterValidator()
    {
        RuleFor(x => x.CurrentPage).GreaterThan(0);
        RuleFor(x => x.PageSize).GreaterThan(0);
    }
}

/// <inheritdoc />
public class ScalendarFilterHandler : IRequestHandler<ScalendarFilterQuery, ScalendarFilterResponse>
{
    private readonly IMapper _mapper;
    private readonly IScalendarService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public ScalendarFilterHandler(IScalendarService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<ScalendarFilterResponse> Handle(ScalendarFilterQuery request,
        CancellationToken cancellationToken = default)
    {
        var spec = new ScalendarFilterSpec(request.Filter);
        var entities = await _service.FilterSpec(request.CurrentPage, request.PageSize, spec, cancellationToken);
        return new ScalendarFilterResponse { Data = _mapper.Map<Pager<ScalendarModel>>(entities) };
    }
}

/// <inheritdoc />
public record ScalendarFilterResponse : CQRSResponse<Pager<ScalendarModel>>;