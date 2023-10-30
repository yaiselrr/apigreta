using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;

namespace Greta.BO.BusinessLogic.Handlers.Queries.CutList;

/// <summary>
/// Get ScaleProduct by upc and plu
/// </summary>
/// <param name="CutListTemplateId"></param>
/// <param name="AnimalId">Animal Id</param>
public record CutListGetScaleProductOfTemplateQuery(long CutListTemplateId, long AnimalId) : IRequest<CutListGetScaleProductOfTemplateResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"add_edit_{nameof(Animal).ToLower()}")
    };    
}

/// <inheritdoc />
public class CutListGetScaleProductOfTemplateHandler : IRequestHandler<CutListGetScaleProductOfTemplateQuery, CutListGetScaleProductOfTemplateResponse>
{
    private readonly IMapper _mapper;
    private readonly ICutListService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public CutListGetScaleProductOfTemplateHandler(ICutListService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<CutListGetScaleProductOfTemplateResponse> Handle(CutListGetScaleProductOfTemplateQuery request, CancellationToken cancellationToken = default)
    {
        var entity = await _service.GetScaleProductsOfTemplate(request.CutListTemplateId, request.AnimalId);
        var data = _mapper.Map<List<ScaleProductLiteModel>>(entity);
        return new CutListGetScaleProductOfTemplateResponse { Data = data };
    }
}

/// <inheritdoc />
public record CutListGetScaleProductOfTemplateResponse : CQRSResponse<List<ScaleProductLiteModel>>;