using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;

namespace Greta.BO.BusinessLogic.Handlers.Queries.ScaleLabelType;

/// <summary>
/// Get all entities
/// </summary>
public record ScaleLabelTypeGetAllQuery : IRequest<ScaleLabelTypeGetAllResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
            {
                new PermissionRequirement.Requirement($"view_scale_label_type")
            };
}

/// <inheritdoc />
public class ScaleLabelTypeGetAllHandler : IRequestHandler<ScaleLabelTypeGetAllQuery, ScaleLabelTypeGetAllResponse>
{
    private readonly IMapper _mapper;
    private readonly IScaleLabelTypeService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public ScaleLabelTypeGetAllHandler(IScaleLabelTypeService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<ScaleLabelTypeGetAllResponse> Handle(ScaleLabelTypeGetAllQuery request, CancellationToken cancellationToken = default)
    {
        var entities = await _service.Get();
        return new ScaleLabelTypeGetAllResponse { Data = _mapper.Map<List<ScaleLabelTypeModel>>(entities) };
    }
}

/// <inheritdoc />
public record ScaleLabelTypeGetAllResponse : CQRSResponse<List<ScaleLabelTypeModel>>;