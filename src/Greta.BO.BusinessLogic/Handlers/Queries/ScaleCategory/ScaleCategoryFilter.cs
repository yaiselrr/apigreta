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
using Greta.BO.BusinessLogic.Specifications.ScaleCategorySpecs;
using Greta.Sdk.Core.Models.Pager;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Queries.ScaleCategory;

/// <summary>
/// Query for filter the ScaleCategory entities
/// </summary>
/// <param name="CurrentPage">Current page</param>
/// <param name="PageSize">Current size</param>
/// <param name="Filter">Filter object of extend of type <see cref="BaseSearchModel"/></param>
public record ScaleCategoryFilterQuery
    (int CurrentPage, int PageSize, ScaleCategorySearchModel Filter) : IRequest<ScaleCategoryFilterResponse>,
        IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"view_scale_category")
    };
}

/// <inheritdoc />
public class ScaleCategoryFilterValidator : AbstractValidator<ScaleCategoryFilterQuery>
{
    /// <inheritdoc />
    public ScaleCategoryFilterValidator()
    {
        RuleFor(x => x.CurrentPage).GreaterThan(0);
        RuleFor(x => x.PageSize).GreaterThan(0);
    }
}

/// <inheritdoc />
public class ScaleCategoryFilterHandler : IRequestHandler<ScaleCategoryFilterQuery, ScaleCategoryFilterResponse>
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly IScaleCategoryService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public ScaleCategoryFilterHandler(ILogger<ScaleCategoryFilterHandler> logger, IScaleCategoryService service,
        IMapper mapper)
    {
        _logger = logger;
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<ScaleCategoryFilterResponse> Handle(ScaleCategoryFilterQuery request,
        CancellationToken cancellationToken = default)
    {
        if (request.CurrentPage < 1 || request.PageSize < 1)
        {
            _logger.LogError("Page parameter (currentPage or pageSize) out of bounds");
            throw new BusinessLogicException("Page parameter out of bounds.");
        }
        
        var spec = new ScaleCategoryFilterSpec(request.Filter);
        var entities = await _service.FilterSpec(request.CurrentPage, request.PageSize, spec, cancellationToken);
        return new ScaleCategoryFilterResponse { Data = _mapper.Map<Pager<ScaleCategoryModel>>(entities) };
    }
}

/// <inheritdoc />
public record ScaleCategoryFilterResponse : CQRSResponse<Pager<ScaleCategoryModel>>;