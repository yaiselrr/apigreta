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

namespace Greta.BO.BusinessLogic.Handlers.Queries.VendorOrderDetail;

/// <inheritdoc />
public record VendorOrderDetailGetAllQuery : IRequest<VendorOrderDetailGetAllResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"add_edit_vendor_order")
    };
}

/// <inheritdoc />
public class VendorOrderDetailGetAllHandler : IRequestHandler<VendorOrderDetailGetAllQuery, VendorOrderDetailGetAllResponse>
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly IVendorOrderDetailService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public VendorOrderDetailGetAllHandler(ILogger<VendorOrderDetailGetAllHandler> logger, IVendorOrderDetailService service, IMapper mapper)
    {
        _logger = logger;
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<VendorOrderDetailGetAllResponse> Handle(VendorOrderDetailGetAllQuery request,
        CancellationToken cancellationToken)
    {
        var entities = await _service.Get();
        return new VendorOrderDetailGetAllResponse { Data = _mapper.Map<List<VendorOrderDetailModel>>(entities) };
    }
}

/// <inheritdoc />
public record VendorOrderDetailGetAllResponse : CQRSResponse<List<VendorOrderDetailModel>>;