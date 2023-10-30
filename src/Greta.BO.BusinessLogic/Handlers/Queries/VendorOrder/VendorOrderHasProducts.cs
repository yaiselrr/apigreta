using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Queries.VendorOrder;

/// <summary>
/// Returns true if vendor order have products associated
/// </summary>
/// <param name="VendorOrderId">Vendor Order Id</param>
public record VendorOrderHasProductsQuery(long VendorOrderId) : IRequest<VendorOrderHasProductsResponse>;

/// <inheritdoc />
public record VendorOrderHasProductsResponse : CQRSResponse<bool>;

/// <inheritdoc />
public class VendorOrderHasProductsHandler : IRequestHandler<VendorOrderHasProductsQuery, VendorOrderHasProductsResponse>
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
    public VendorOrderHasProductsHandler(ILogger<VendorOrderHasProductsHandler> logger, IVendorOrderService service, IMapper mapper)
    {
        _logger = logger;
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<VendorOrderHasProductsResponse> Handle(VendorOrderHasProductsQuery request,
        CancellationToken cancellationToken = default)
    {
        var data = await _service.HasProducts(request.VendorOrderId);
        return new VendorOrderHasProductsResponse() { Data = data };
    }
}