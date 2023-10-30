using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Queries.VendorOrder;

/// <summary>
/// Query for return all vendor orders
/// </summary>
public record VendorOrderGetAllQuery : IRequest<VendorOrderGetAllResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"view_vendor_order")
    };
}

/// <inheritdoc />
public class VendorOrderGetAllHandler : IRequestHandler<VendorOrderGetAllQuery, VendorOrderGetAllResponse>
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
    public VendorOrderGetAllHandler(ILogger<VendorOrderGetAllHandler> logger, IVendorOrderService service, IMapper mapper)
    {
        _logger = logger;
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<VendorOrderGetAllResponse> Handle(VendorOrderGetAllQuery request,
        CancellationToken cancellationToken)
    {
        var entities = await _service.Get();
        return new VendorOrderGetAllResponse { Data = _mapper.Map<List<VendorModel>>(entities) };
    }
}

/// <inheritdoc />
public record VendorOrderGetAllResponse : CQRSResponse<List<VendorModel>>;