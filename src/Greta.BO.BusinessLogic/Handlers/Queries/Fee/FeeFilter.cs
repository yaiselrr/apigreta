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
using Greta.BO.BusinessLogic.Specifications.FeeSpecs;
using Greta.Sdk.Core.Models.Pager;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Queries.Fee;

/// <summary>
/// Query for filter an paginate Fee
/// </summary>
/// <param name="CurrentPage"></param>
/// <param name="PageSize"></param>
/// <param name="Filter"></param>
public record FeeFilterQuery(int CurrentPage, int PageSize, FeeSearchModel Filter) : IRequest<FeeFilterResponse>, IAuthorizable
{
    /// <summary>
    /// 
    /// </summary>
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"view_{nameof(Fee).ToLower()}")
    };
}

///<inheritdoc/>
public class FeeFilterValidator : AbstractValidator<FeeFilterQuery>
{
    /// <summary>
    /// 
    /// </summary>
    public FeeFilterValidator()
    {
        RuleFor(x => x.CurrentPage).GreaterThan(0);
        RuleFor(x => x.PageSize).GreaterThan(0);
    }
}

///<inheritdoc/>
public class FeeFilterHandler : IRequestHandler<FeeFilterQuery, FeeFilterResponse>
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly IFeeService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public FeeFilterHandler(ILogger<FeeFilterHandler> logger, IFeeService service, IMapper mapper)
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
    public async Task<FeeFilterResponse> Handle(FeeFilterQuery request, CancellationToken cancellationToken = default)
    {
        if (request.CurrentPage < 1 || request.PageSize < 1)
        {
            _logger.LogError("Page parameter (currentPage or pageSize) out of bounds");
            throw new BusinessLogicException("Page parameter out of bounds");
        }

        var spec = new FeeFilterSpec(request.Filter);
        var entities = await _service.FilterSpec(request.CurrentPage, request.PageSize, spec, cancellationToken);           
        return new FeeFilterResponse { Data = _mapper.Map<Pager<FeeFilterModel>>(entities)};
    }
}

///<inheritdoc/>
public record FeeFilterResponse : CQRSResponse<Pager<FeeFilterModel>>;
