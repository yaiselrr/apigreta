using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Queries.VendorOrderDetail;

/// <inheritdoc />
public record GetStatusPurchaseOrderQuery(long VendorOrderId) : IRequest<GetStatusPurchaseOrderResponse>;

/// <inheritdoc />
public record GetStatusPurchaseOrderResponse : CQRSResponse<int>;

/// <inheritdoc />
public class GetStatusPurchaseOrderHandler : IRequestHandler<GetStatusPurchaseOrderQuery, GetStatusPurchaseOrderResponse>
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
    public GetStatusPurchaseOrderHandler(ILogger<GetStatusPurchaseOrderHandler> logger, IVendorOrderDetailService service, IMapper mapper)
    {
        _logger = logger;
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<GetStatusPurchaseOrderResponse> Handle(GetStatusPurchaseOrderQuery request,
        CancellationToken cancellationToken)
    {
        var data = await _service.GetStatusPurchaseOrder(request.VendorOrderId);
        return new GetStatusPurchaseOrderResponse() { Data = data };
    }
}