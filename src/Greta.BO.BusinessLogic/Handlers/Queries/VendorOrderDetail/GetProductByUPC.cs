using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Queries.VendorOrderDetail;

/// <inheritdoc />
public record GetProductByUpcQuery(long StoreId, long VendorId, string Upc) : IRequest<GetProductByUpcResponse>;

/// <inheritdoc />
public record GetProductByUpcResponse : CQRSResponse<StoreProductMinimalModel>;

/// <inheritdoc />
public class GetProductByUpcHandler : IRequestHandler<GetProductByUpcQuery, GetProductByUpcResponse>
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
    public GetProductByUpcHandler(ILogger<GetProductByUpcHandler> logger, IVendorOrderDetailService service,
        IMapper mapper)
    {
        _logger = logger;
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<GetProductByUpcResponse> Handle(GetProductByUpcQuery request, CancellationToken cancellationToken)
    {
        var storeProduct = await _service.GetStoreProductByUpc(request.StoreId, request.VendorId, request.Upc);
        var data = this._mapper.Map<StoreProductMinimalModel>(storeProduct);
        if (data != null)
        {
            data.CasePack = storeProduct.Product.VendorProducts[0].CasePack;
            data.CaseCost = storeProduct.Product.VendorProducts[0].CaseCost;
            if(storeProduct.Product.VendorProducts[0].PackSize != null)
                data.PackSize = storeProduct.Product.VendorProducts[0].PackSize;
            data.ProductCode = storeProduct.Product.VendorProducts[0].ProductCode;
        }

        return new GetProductByUpcResponse() { Data = data };
    }
}