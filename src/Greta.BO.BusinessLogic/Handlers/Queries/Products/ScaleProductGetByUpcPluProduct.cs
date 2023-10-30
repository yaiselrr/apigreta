using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Models.Dto.Search;
using Greta.BO.BusinessLogic.Service;
using Greta.Sdk.Core.Models.Pager;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Queries.Products;

/// <summary>
/// Get filter and paginate Scale product without cutListTemplate
/// </summary>
/// <param name="cutTemplateId"></param>
/// <param name="currentPage"></param>
/// <param name="pageSize"></param>
/// <param name="filter"></param>
public record ScaleProductGetByUpcPluProductQuery(long cutTemplateId,int currentPage, int pageSize, ScaleProductSearchModel filter) : IRequest<ScaleProductGetByUpcPluProductResponse>, IAuthorizable
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
public class ScaleProductGetByUpcPluProductHandler : IRequestHandler<ScaleProductGetByUpcPluProductQuery, ScaleProductGetByUpcPluProductResponse>
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
    public ScaleProductGetByUpcPluProductHandler(ILogger<ScaleProductGetByUpcPluProductHandler> logger, IProductService service, IMapper mapper)
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
    public async Task<ScaleProductGetByUpcPluProductResponse> Handle(ScaleProductGetByUpcPluProductQuery request, CancellationToken cancellationToken = default)
    {
        var entities = await _service.GetScaleProductsByUpcPluProduct(request.cutTemplateId,request.currentPage, request.pageSize, request.filter);
        var result = _mapper.Map<Pager<ScaleProductLiteModel>>(entities);
        return new ScaleProductGetByUpcPluProductResponse { Data = result };        
    }
}

///<inheritdoc/>
public record ScaleProductGetByUpcPluProductResponse : CQRSResponse<Pager<ScaleProductLiteModel>>;
