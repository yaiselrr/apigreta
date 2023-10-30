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
using Greta.BO.BusinessLogic.Specifications.ExternalScaleSpecs;
using Greta.Sdk.Core.Models.Pager;
using MediatR;

namespace Greta.BO.BusinessLogic.Handlers.Queries.ExternalScale;

/// <summary>
/// Query for filter the ExternalScale entities
/// </summary>
/// <param name="CurrentPage">Current page</param>
/// <param name="PageSize">Current size</param>
/// <param name="Filter">Filter object of extend of type <see cref="BaseSearchModel"/></param>
public record ExternalScaleFilterQuery
    (int CurrentPage, int PageSize, ExternalScaleSearchModel Filter) : IRequest<ExternalScaleFilterResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"view_external_scale")
    };
}

/// <inheritdoc />
public class ExternalScaleFilterValidator : AbstractValidator<ExternalScaleFilterQuery>
{
    /// <inheritdoc />
    public ExternalScaleFilterValidator()
    {
        RuleFor(x => x.CurrentPage).GreaterThan(0);
        RuleFor(x => x.PageSize).GreaterThan(0);
    }
}

/// <inheritdoc />
public class ExternalScaleFilterHandler : IRequestHandler<ExternalScaleFilterQuery, ExternalScaleFilterResponse>
{
    private readonly IMapper _mapper;
    private readonly IExternalScaleService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public ExternalScaleFilterHandler(IExternalScaleService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<ExternalScaleFilterResponse> Handle(ExternalScaleFilterQuery request,
        CancellationToken cancellationToken = default)
    {
        var spec = new ExternalScaleFilterSpec(request.Filter);
        var entities = await _service.FilterSpec(request.CurrentPage, request.PageSize, spec, cancellationToken);
        return new ExternalScaleFilterResponse { Data = _mapper.Map<Pager<ExternalScaleModel>>(entities) };
    }
}

/// <inheritdoc />
public record ExternalScaleFilterResponse : CQRSResponse<Pager<ExternalScaleModel>>;