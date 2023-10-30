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
using Greta.BO.BusinessLogic.Specifications.ScaleLabelTypeSpecs;
using Greta.Sdk.Core.Models.Pager;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Queries.ScaleLabelType;

/// <summary>
/// Query for filter the ScaleLabelType entities
/// </summary>
/// <param name="CurrentPage">Current page</param>
/// <param name="PageSize">Current size</param>
/// <param name="Filter">Filter object of extend of type <see cref="BaseSearchModel"/></param>
public record ScaleLabelTypeFilterQuery
    (int CurrentPage, int PageSize, ScaleLabelTypeSearchModel Filter) : IRequest<ScaleLabelTypeFilterResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"view_scale_label_type")
    };
}

/// <inheritdoc />
public record ScaleLabelTypeFilterResponse : CQRSResponse<Pager<ScaleLabelTypeListModel>>;

/// <inheritdoc />
public class ScaleLabelTypeFilterValidator : AbstractValidator<ScaleLabelTypeFilterQuery>
{
    /// <inheritdoc />
    public ScaleLabelTypeFilterValidator()
    {
        RuleFor(x => x.CurrentPage).GreaterThan(0);
        RuleFor(x => x.PageSize).GreaterThan(0);
    }
}

/// <inheritdoc />
public class ScaleLabelTypeFilterHandler : IRequestHandler<ScaleLabelTypeFilterQuery, ScaleLabelTypeFilterResponse>
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly IScaleLabelTypeService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public ScaleLabelTypeFilterHandler(ILogger<ScaleLabelTypeFilterHandler> logger, IScaleLabelTypeService service, IMapper mapper)
    {
        _logger = logger;
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<ScaleLabelTypeFilterResponse> Handle(ScaleLabelTypeFilterQuery request, CancellationToken cancellationToken = default)
    {
        var spec = new ScaleLabelTypeFilterSpec(request.Filter);

        var data = await _service.FilterSpec(
            request.CurrentPage,
            request.PageSize,
            spec,
            cancellationToken
        );
        return new ScaleLabelTypeFilterResponse { Data = _mapper.Map<Pager<ScaleLabelTypeListModel>>(data) };
    }
}