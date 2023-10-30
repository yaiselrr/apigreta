// Ignore Spelling: Validator

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Models.Dto.Search;
using Greta.BO.BusinessLogic.Service;
using Greta.BO.BusinessLogic.Specifications.RoleSpecs;
using Greta.Sdk.Core.Models.Pager;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Queries.Role;

/// <summary>
/// Query for filter the rol entities
/// </summary>
/// <param name="CurrentPage">Current page</param>
/// <param name="PageSize">Current size</param>
/// <param name="Filter">Filter object of extend of type <see cref="BaseSearchModel"/></param>
public record RoleFilterQuery(int CurrentPage, int PageSize, RoleSearchModel Filter) : IRequest<RoleFilterResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement(string.Format("view_{0}",nameof(Role).ToLower()))
    };
}

/// <inheritdoc />
public class RoleFilterValidator : AbstractValidator<RoleFilterQuery>
{
    /// <inheritdoc />
    public RoleFilterValidator()
    {
        RuleFor(x => x.CurrentPage).GreaterThan(0);
        RuleFor(x => x.PageSize).GreaterThan(0);
    }
}

/// <inheritdoc />
public class RoleFilterHandler : IRequestHandler<RoleFilterQuery, RoleFilterResponse>
{

    private readonly ILogger _logger;

    private readonly IMapper _mapper;

    private readonly IRoleService _service;

    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public RoleFilterHandler(ILogger<RoleFilterHandler> logger, IRoleService service, IMapper mapper)
    {
        _logger = logger;
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<RoleFilterResponse> Handle(RoleFilterQuery request, CancellationToken cancellationToken = default)
    {
        if (request.CurrentPage < 1 || request.PageSize < 1)
        {
            _logger.LogError("Page parameter (currentPage or pageSize) out of bounds");
            throw new BusinessLogicException("Page parameter out of bounds");
        }
        var spec = new RoleFilterSpec(request.Filter);
        var result = await _service.FilterSpec(request.CurrentPage, request.PageSize, spec, cancellationToken);       
        return new RoleFilterResponse { Data = _mapper.Map<Pager<RoleModel>>(result) };
    }
}

/// <inheritdoc />
public record RoleFilterResponse : CQRSResponse<Pager<RoleModel>>;
