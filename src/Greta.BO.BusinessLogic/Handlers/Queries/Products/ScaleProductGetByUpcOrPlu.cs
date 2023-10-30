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
/// <param name="Filter"></param>
public record ScaleProductGetByUpcOrPluQuery(string Filter) : IRequest<ScaleProductGetByUpcOrPluResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"view_product")
    };
}

/// <inheritdoc />
public class ScaleProductGetByUpcOrPluHandler : IRequestHandler<ScaleProductGetByUpcOrPluQuery, ScaleProductGetByUpcOrPluResponse>
{
    private readonly IMapper _mapper;
    private readonly IProductService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public ScaleProductGetByUpcOrPluHandler( IProductService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<ScaleProductGetByUpcOrPluResponse> Handle(ScaleProductGetByUpcOrPluQuery request, CancellationToken cancellationToken = default)
    {
        var data = await _service.GetScaleProductsByUpcOrPlu(request.Filter);
        return new ScaleProductGetByUpcOrPluResponse { Data = _mapper.Map<List<ScaleProductLiteModel>>(data)};
    }
}

/// <inheritdoc />
public record ScaleProductGetByUpcOrPluResponse : CQRSResponse<List<ScaleProductLiteModel>>;