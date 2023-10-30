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
using Greta.BO.BusinessLogic.Specifications.BreedSpecs;
using Greta.Sdk.Core.Models.Pager;
using MediatR;

namespace Greta.BO.BusinessLogic.Handlers.Queries.Breed;

/// <summary>
/// Query for filter the Breed entities
/// </summary>
/// <param name="CurrentPage">Current page</param>
/// <param name="PageSize">Current size</param>
/// <param name="Filter">Filter object of extend of type <see cref="BaseSearchModel"/></param>
public record BreedFilterQuery
    (int CurrentPage, int PageSize, BreedSearchModel Filter) : IRequest<BreedFilterResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"view_{nameof(Breed).ToLower()}")
    };
}

/// <inheritdoc />
public class BreedFilterValidator : AbstractValidator<BreedFilterQuery>
{
    /// <inheritdoc />
    public BreedFilterValidator()
    {
        RuleFor(x => x.CurrentPage).GreaterThan(0);
        RuleFor(x => x.PageSize).GreaterThan(0);
    }
}

/// <inheritdoc />
public class BreedFilterHandler : IRequestHandler<BreedFilterQuery, BreedFilterResponse>
{
    private readonly IMapper _mapper;
    private readonly IBreedService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public BreedFilterHandler(IBreedService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<BreedFilterResponse> Handle(BreedFilterQuery request,
        CancellationToken cancellationToken = default)
    {
        var spec = new BreedFilterSpec(request.Filter);
        var entities = await _service.FilterSpec(request.CurrentPage, request.PageSize, spec, cancellationToken);
        return new BreedFilterResponse { Data = _mapper.Map<Pager<BreedModel>>(entities) };
    }
}

/// <inheritdoc />
public record BreedFilterResponse : CQRSResponse<Pager<BreedModel>>;