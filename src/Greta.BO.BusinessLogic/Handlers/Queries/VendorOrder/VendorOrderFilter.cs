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
using Greta.BO.BusinessLogic.Specifications.VendorOrderSpecs;
using Greta.Sdk.Core.Models.Pager;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Queries.VendorOrder;

/// <summary>
/// Query for filter the vendor order entities
/// </summary>
/// <param name="CurrentPage">Current page</param>
/// <param name="PageSize">Current size</param>
/// <param name="Filter">Filter object of extend of type <see cref="BaseSearchModel"/></param>
public record VendorOrderFilterQuery
    (int CurrentPage, int PageSize, VendorOrderSearchModel Filter) : IRequest<VendorOrderFilterResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"view_vendor_order")
    };
}

/// <inheritdoc />
public class VendorOrderFilterValidator : AbstractValidator<VendorOrderFilterQuery>
{
    /// <inheritdoc />
    public VendorOrderFilterValidator()
    {
        RuleFor(x => x.CurrentPage).GreaterThan(0);
        RuleFor(x => x.PageSize).GreaterThan(0);
    }
}

/// <inheritdoc />
public class VendorOrderFilterHandler : IRequestHandler<VendorOrderFilterQuery, VendorOrderFilterResponse>
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly IVendorOrderService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public VendorOrderFilterHandler(ILogger<VendorOrderFilterHandler> logger, IVendorOrderService service, IMapper mapper)
    {
        _logger = logger;
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<VendorOrderFilterResponse> Handle(VendorOrderFilterQuery request,
        CancellationToken cancellationToken = default)
    {
        if (request.CurrentPage < 1 || request.PageSize < 1)
        {
            _logger.LogError("Page parameter (currentPage or pageSize) out of bounds");
            throw new BusinessLogicException("Page parameter out of bounds.");
        }
        
        var spec = new VendorOrderFilterSpec(request.Filter);
        var entities = await _service.FilterSpec(request.CurrentPage, request.PageSize, spec, cancellationToken);
        return new VendorOrderFilterResponse { Data = _mapper.Map<Pager<VendorOrderModel>>(entities) };
    }
}

/// <inheritdoc />
public record VendorOrderFilterResponse : CQRSResponse<Pager<VendorOrderModel>>;