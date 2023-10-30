using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Core.Caching;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;

namespace Greta.BO.BusinessLogic.Handlers.Queries.ScaleLabelType;

/// <summary>
/// Get entity by type
/// </summary>
/// <param name="type">ScaleType</param>
public record ScaleLabelTypeGetByTypeQuery(ScaleType type) : IRequest<ScaleLabelTypeGetByTypeResponse>, IAuthorizable, ICacheable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new() {
                new PermissionRequirement.Requirement($"view_scale_label_type")
    };

    /// <inheritdoc />
    public string CacheKey => $"ScaleLabelTypeGetByType{type}";
}

/// <inheritdoc />
public class ScaleLabelTypeGetByTypeHandler : IRequestHandler<ScaleLabelTypeGetByTypeQuery, ScaleLabelTypeGetByTypeResponse>
{
    private readonly IMapper _mapper;
    private readonly IScaleLabelTypeService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public ScaleLabelTypeGetByTypeHandler(IScaleLabelTypeService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<ScaleLabelTypeGetByTypeResponse> Handle(ScaleLabelTypeGetByTypeQuery request, CancellationToken cancellationToken = default)
    {
        var entity = await _service.GetByType(request.type);
        return new ScaleLabelTypeGetByTypeResponse { Data = _mapper.Map<List<ScaleLabelTypeModel>>(entity) };
    }
}

/// <inheritdoc />
public record ScaleLabelTypeGetByTypeResponse : CQRSResponse<List<ScaleLabelTypeModel>>;