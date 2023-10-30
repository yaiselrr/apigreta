using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Models.Dto.Search;
using Greta.BO.BusinessLogic.Service;
using Greta.BO.BusinessLogic.Specifications.ScaleProductSpecs;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Queries.Products;

/// <summary>
/// Get scale product by cut list template Id
/// </summary>
public record ScaleProductGetByCutListTemplateQuery(long TemplateId) : IRequest<ScaleProductGetByCutListTemplateResponse>, IAuthorizable
{
    /// <summary>
    /// 
    /// </summary>
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"view_product")
    };
}

///<inheritdoc/>
public class ScaleProductGetByCutListTemplateHandler : IRequestHandler<ScaleProductGetByCutListTemplateQuery, ScaleProductGetByCutListTemplateResponse>
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly IProductService _service;   

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public ScaleProductGetByCutListTemplateHandler(ILogger<ScaleProductGetByCutListTemplateHandler> logger, IProductService service, IMapper mapper)
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
    public async Task<ScaleProductGetByCutListTemplateResponse> Handle(ScaleProductGetByCutListTemplateQuery request, CancellationToken cancellationToken = default)
    {
        var entities = await _service.GetScaleProductsByTemplate(request.TemplateId);
        var result = _mapper.Map<List<ScaleProductLiteModel>>(entities);
        return new ScaleProductGetByCutListTemplateResponse { Data = result };
    }
}

///<inheritdoc/>
public record ScaleProductGetByCutListTemplateResponse : CQRSResponse<List<ScaleProductLiteModel>>;
