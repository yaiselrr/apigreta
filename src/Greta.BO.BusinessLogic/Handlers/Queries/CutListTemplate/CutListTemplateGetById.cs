using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Core.Caching;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using Greta.BO.BusinessLogic.Specifications.CutListTemplateSpecs;
using MediatR;

namespace Greta.BO.BusinessLogic.Handlers.Queries.CutListTemplate;

/// <summary>
/// Get entity by id
/// </summary>
/// <param name="Id">Tax id</param>
public record CutListTemplateGetByIdQuery(long Id) : IRequest<CutListTemplateGetByIdResponse>, IAuthorizable, ICacheable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"add_edit_{nameof(Animal).ToLower()}")
    };

    /// <inheritdoc />
    public string CacheKey => $"CutListTemplateGetById{Id}";
}

/// <inheritdoc />
public class CutListTemplateGetByIdHandler : IRequestHandler<CutListTemplateGetByIdQuery, CutListTemplateGetByIdResponse>
{
    private readonly IMapper _mapper;
    private readonly ICutListTemplateService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public CutListTemplateGetByIdHandler(ICutListTemplateService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<CutListTemplateGetByIdResponse> Handle(CutListTemplateGetByIdQuery request, CancellationToken cancellationToken = default)
    {
        var entity = await _service.Get(new CutListTemplateGetByIdSpec(request.Id), cancellationToken);
        var data = _mapper.Map<CutListTemplateModel>(entity.FirstOrDefault());
        return data == null ? null : new CutListTemplateGetByIdResponse { Data = data };
    }
}

/// <inheritdoc />
public record CutListTemplateGetByIdResponse : CQRSResponse<CutListTemplateModel>;