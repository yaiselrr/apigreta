using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;

namespace Greta.BO.BusinessLogic.Handlers.Queries.CutListTemplate;

/// <summary>
/// Get all entities
/// </summary>
public record CutListTemplateGetAllQuery : IRequest<CutListTemplateGetAllResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"add_edit_{nameof(Animal).ToLower()}")
    };
}

/// <inheritdoc />
public class CutListTemplateGetAllHandler : IRequestHandler<CutListTemplateGetAllQuery, CutListTemplateGetAllResponse>
{
    private readonly IMapper _mapper;
    private readonly ICutListTemplateService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public CutListTemplateGetAllHandler(ICutListTemplateService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<CutListTemplateGetAllResponse> Handle(CutListTemplateGetAllQuery request, CancellationToken cancellationToken = default)
    {
        var entities = await _service.Get();
        return new CutListTemplateGetAllResponse { Data = _mapper.Map<List<CutListTemplateModel>>(entities) };
    }
}

/// <inheritdoc />
public record CutListTemplateGetAllResponse : CQRSResponse<List<CutListTemplateModel>>;