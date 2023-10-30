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
/// Query for filter the family entities
/// </summary>
/// <param name="CurrentPage">Current page</param>
/// <param name="PageSize">Current size</param>
/// <param name="Filter">Filter object of extend of type <see cref="BaseSearchModel"/></param>
public record FamilyFilterQuery
    (int CurrentPage, int PageSize, FamilySearchModel Filter) : IRequest<FamilyFilterResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"view_{nameof(Family).ToLower()}")
    };
}

/// <inheritdoc />
public class FamilyFilterValidator : AbstractValidator<FamilyFilterQuery>
{
    /// <inheritdoc />
    public FamilyFilterValidator()
    {
        RuleFor(x => x.CurrentPage).GreaterThan(0);
        RuleFor(x => x.PageSize).GreaterThan(0);
    }
}

/// <inheritdoc />
public class FamilyFilterHandler : IRequestHandler<FamilyFilterQuery, FamilyFilterResponse>
{
    private readonly IMapper _mapper;
    private readonly IFamilyService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public FamilyFilterHandler(IFamilyService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<FamilyFilterResponse> Handle(FamilyFilterQuery request, CancellationToken cancellationToken = default)
    {
        var spec = new FamilyFilterSpec(request.Filter);
        var entities = await _service.FilterSpec(request.CurrentPage, request.PageSize, spec, cancellationToken);
        return new FamilyFilterResponse { Data = _mapper.Map<Pager<FamilyModel>>(entities) };
    }
}

/// <inheritdoc />
public record FamilyFilterResponse : CQRSResponse<Pager<FamilyModel>>;