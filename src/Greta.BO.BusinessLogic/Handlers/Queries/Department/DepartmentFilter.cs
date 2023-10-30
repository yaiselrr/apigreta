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
using Greta.BO.BusinessLogic.Specifications.DepartmentSpecs;
using Greta.Sdk.Core.Models.Pager;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Queries.Department;

/// <summary>
/// Query for filter the department entities
/// </summary>
/// <param name="CurrentPage">Current page</param>
/// <param name="PageSize">Current size</param>
/// <param name="Filter">Filter object of extend of type <see cref="BaseSearchModel"/></param>
public record DepartmentFilterQuery
    (int CurrentPage, int PageSize, DepartmentSearchModel Filter) : IRequest<DepartmentFilterResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"view_{nameof(Department).ToLower()}")
    };
}

/// <inheritdoc />
public class DepartmentFilterValidator : AbstractValidator<DepartmentFilterQuery>
{
    /// <inheritdoc />
    public DepartmentFilterValidator()
    {
        RuleFor(x => x.CurrentPage).GreaterThan(0);
        RuleFor(x => x.PageSize).GreaterThan(0);
    }
}

/// <inheritdoc />
public class DepartmentFilterHandler : IRequestHandler<DepartmentFilterQuery, DepartmentFilterResponse>
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly IDepartmentService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public DepartmentFilterHandler(ILogger<DepartmentFilterHandler> logger, IDepartmentService service, IMapper mapper)
    {
        _logger = logger;
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<DepartmentFilterResponse> Handle(DepartmentFilterQuery request, CancellationToken cancellationToken = default)
    {
        var spec = new DepartmentFilterSpec(request.Filter);
        var entities = await _service.FilterSpec(request.CurrentPage, request.PageSize, spec, cancellationToken);
        return new DepartmentFilterResponse { Data = _mapper.Map<Pager<DepartmentModel>>(entities) };
    }
}

/// <inheritdoc />
public record DepartmentFilterResponse : CQRSResponse<Pager<DepartmentModel>>;