using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;

namespace Greta.BO.BusinessLogic.Handlers.Queries.Products;

/// <summary>
/// Get a ProductModel by upc
/// </summary>
/// <param name="UPC"></param>
public record ProductGetByUpcQuery(string Upc) : IRequest<ProductGetByUpcResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"view_product")
    };
}

/// <inheritdoc />
public class ProductGetByUpcHandler : IRequestHandler<ProductGetByUpcQuery, ProductGetByUpcResponse>
{
    private readonly IMapper _mapper;
    private readonly IProductService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public ProductGetByUpcHandler( IProductService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<ProductGetByUpcResponse> Handle(ProductGetByUpcQuery request, CancellationToken cancellationToken)
    {
        var data = await _service.GetProductByUPC(request.Upc);
        return new ProductGetByUpcResponse {Data = _mapper.Map<ProductModel>(data)};
    }
}

/// <inheritdoc />
public record ProductGetByUpcResponse : CQRSResponse<ProductModel>;