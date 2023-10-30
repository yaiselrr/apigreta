using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Core.Caching;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;

namespace Greta.BO.BusinessLogic.Handlers.Queries.ScaleLabelType;

/// <summary>
/// Get entity by id
/// </summary>
/// <param name="Id">ScaleLabelType id</param>
public record ScaleLabelTypeGetByIdQuery(long Id) : IRequest<ScaleLabelTypeGetByIdResponse>, IAuthorizable, ICacheable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new() {
                new PermissionRequirement.Requirement($"view_scale_label_type")
    };

    /// <inheritdoc />
    public string CacheKey => $"ScaleLabelTypeGetById{Id}";
}

/// <inheritdoc />
public class ScaleLabelTypeGetByIdHandler : IRequestHandler<ScaleLabelTypeGetByIdQuery, ScaleLabelTypeGetByIdResponse>
{
    private readonly IMapper _mapper;
    private readonly IScaleLabelTypeService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public ScaleLabelTypeGetByIdHandler(IScaleLabelTypeService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<ScaleLabelTypeGetByIdResponse> Handle(ScaleLabelTypeGetByIdQuery request, CancellationToken cancellationToken = default)
    {
        var entity = await _service.Get(request.Id);
        var data = _mapper.Map<ScaleLabelTypeModel>(entity);
        return data == null ? null : new ScaleLabelTypeGetByIdResponse { Data = data };
    }
}

/// <inheritdoc />
public record ScaleLabelTypeGetByIdResponse : CQRSResponse<ScaleLabelTypeModel>;