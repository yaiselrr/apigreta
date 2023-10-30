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
using Greta.Sdk.Core.Models.Pager;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Queries.ScaleLabelDefinition;

public record ScaleLabelDefinitionFilterQuery
    (int CurrentPage, int PageSize, ScaleLabelDefinitionSearchModel Filter) :
        IRequest<ScaleLabelDefinitionFilterResponse>, IAuthorizable
{
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"associate_product_scale_label_definition")
    };
}

public class ScaleLabelDefinitionFilterValidator : AbstractValidator<ScaleLabelDefinitionFilterQuery>
{
    public ScaleLabelDefinitionFilterValidator()
    {
        RuleFor(x => x.CurrentPage).GreaterThan(0);
        RuleFor(x => x.PageSize).GreaterThan(0);
    }
}

public class
    ScaleLabelDefinitionFilterHandler : IRequestHandler<ScaleLabelDefinitionFilterQuery,
        ScaleLabelDefinitionFilterResponse>
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly IScaleLabelDefinitionService _service;

    public ScaleLabelDefinitionFilterHandler(ILogger<ScaleLabelDefinitionFilterHandler> logger,
        IScaleLabelDefinitionService service, IMapper mapper)
    {
        _logger = logger;
        _service = service;
        _mapper = mapper;
    }

    public async Task<ScaleLabelDefinitionFilterResponse> Handle(ScaleLabelDefinitionFilterQuery request,
        CancellationToken cancellationToken)
    {
        if (request.CurrentPage < 1 || request.PageSize < 1)
        {
            _logger.LogError("Page parameter (currentPage or pageSize) out of bounds");
            throw new BusinessLogicException("Page parameter out of bounds.");
        }

        var entities = await _service.Filter(
            request.CurrentPage,
            request.PageSize,
            _mapper.Map<Api.Entities.ScaleLabelDefinition>(request.Filter),
            request.Filter.Search,
            request.Filter.Sort);
        return new ScaleLabelDefinitionFilterResponse
            { Data = _mapper.Map<Pager<ScaleLabelDefinitionModel>>(entities) };
    }
}

public record ScaleLabelDefinitionFilterResponse : CQRSResponse<Pager<ScaleLabelDefinitionModel>>;