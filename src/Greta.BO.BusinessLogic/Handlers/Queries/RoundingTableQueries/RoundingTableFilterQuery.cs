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
using Greta.BO.BusinessLogic.Specifications.RoundingTableSpecs;
using Greta.Sdk.Core.Models.Pager;
using MediatR;

namespace Greta.BO.BusinessLogic.Handlers.Queries.RoundingTableQueries;

/// <summary>
/// Query for filter the Rounding Table entities
/// </summary>
/// <param name="CurrentPage">Current page</param>
/// <param name="PageSize">Current size</param>
/// <param name="Filter">Filter object of extend of type <see cref="BaseSearchModel"/></param>
public record RoundingTableFilterQuery
    (int CurrentPage, int PageSize, RoundingTableSearchModel Filter) : IRequest<RoundingTableFilterResponse>,
        IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements { get; } = new();
}
// {
//     /// <inheritdoc />
//     public List<IRequirement> Requirements => new()
//     {
//         new PermissionRequirement.Requirement($"view_{nameof(RoundingTable).ToLower()}")
//     };
// }

/// <inheritdoc />
public class RoundingTableFilterValidator : AbstractValidator<RoundingTableFilterQuery>
{
    /// <inheritdoc />
    public RoundingTableFilterValidator()
    {
        RuleFor(x => x.CurrentPage).GreaterThan(0);
        RuleFor(x => x.PageSize).GreaterThan(0);
    }
}

/// <inheritdoc />
public class RoundingTableFilterHandler : IRequestHandler<RoundingTableFilterQuery, RoundingTableFilterResponse>
{
    private readonly IMapper _mapper;
    private readonly IRoundingTableService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public RoundingTableFilterHandler(IRoundingTableService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<RoundingTableFilterResponse> Handle(RoundingTableFilterQuery request, CancellationToken cancellationToken = default)
    {
        var spec = new RoundingTableFilterSpec(request.Filter);
        var entities = await _service.FilterSpec(request.CurrentPage, request.PageSize, spec, cancellationToken);
        return new RoundingTableFilterResponse { Data = _mapper.Map<Pager<RoundingTableModel>>(entities) };
    }
}

/// <inheritdoc />
public record RoundingTableFilterResponse : CQRSResponse<Pager<RoundingTableModel>>;