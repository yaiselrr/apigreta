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
public record VendorOrderDetailGetByIdQuery(long Id) : IRequest<VendorOrderDetailGetByIdResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"add_edit_vendor_order")
    };
}

/// <inheritdoc />
public class VendorOrderDetailGetByIdHandler : IRequestHandler<VendorOrderDetailGetByIdQuery, VendorOrderDetailGetByIdResponse>
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
    public VendorOrderDetailGetByIdHandler(ILogger<VendorOrderDetailGetByIdHandler> logger, IVendorOrderDetailService service, IMapper mapper)
    {
        _logger = logger;
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<VendorOrderDetailGetByIdResponse> Handle(VendorOrderDetailGetByIdQuery request,
        CancellationToken cancellationToken)
    {
        var entity = await _service.Get(request.Id);
        var data = _mapper.Map<VendorOrderDetailModel>(entity);
        return new VendorOrderDetailGetByIdResponse { Data = data };
    }
}

/// <inheritdoc />
public record VendorOrderDetailGetByIdResponse : CQRSResponse<VendorOrderDetailModel>;