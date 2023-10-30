using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Models.Dto.MixAndMatchDto;
using Greta.BO.BusinessLogic.Models.Dto.Search;
using Greta.BO.BusinessLogic.Service;
using Greta.BO.BusinessLogic.Specifications.MixAndMatchSpecs;
using Greta.Sdk.Core.Models.Pager;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Queries.MixAndMatch;

/// <summary>
/// Query for filter MixAndMatch entities
/// </summary>
/// <param name="CurrentPage"></param>
/// <param name="PageSize"></param>
/// <param name="Filter"></param>
public record MixAndMatchFilterQuery(int CurrentPage, int PageSize, MixAndMatchSearchModel Filter) : IRequest<MixAndMatchFilterResponse>, IAuthorizable
{
    /// <inheritdoc/>
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"view_mix_and_match")
    };
}

///<inheritdoc/>
public class Validator : AbstractValidator<MixAndMatchFilterQuery>
{
    /// <summary>
    /// 
    /// </summary>
    public Validator()
    {
        RuleFor(x => x.CurrentPage).GreaterThan(0);
        RuleFor(x => x.PageSize).GreaterThan(0);
    }
}

/// <summary>
/// 
/// </summary>
public class MixAndMatchFilterHandler : IRequestHandler<MixAndMatchFilterQuery, MixAndMatchFilterResponse>
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly IMixAndMatchService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public MixAndMatchFilterHandler(ILogger<MixAndMatchFilterHandler> logger, IMixAndMatchService service, IMapper mapper)
    {
        _logger = logger;
        _service = service;
        _mapper = mapper;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="BusinessLogicException"></exception>
    public async Task<MixAndMatchFilterResponse> Handle(MixAndMatchFilterQuery request, CancellationToken cancellationToken = default)
    {
        if (request.CurrentPage < 1 || request.PageSize < 1)
        {
            _logger.LogError("Page parameter (currentPage or pageSize) out of bounds");
            throw new BusinessLogicException("Page parameter out of bounds");
        }
        var spec = new MixAndMatchFilterSpec(request.Filter);
        var entities = await _service.FilterSpec(request.CurrentPage, request.PageSize, spec, cancellationToken);

        return new MixAndMatchFilterResponse { Data = _mapper.Map<Pager<MixAndMatchGetAllModel>>(entities)};
    }
}

///<inheritdoc/>
public record MixAndMatchFilterResponse : CQRSResponse<Pager<MixAndMatchGetAllModel>>;
