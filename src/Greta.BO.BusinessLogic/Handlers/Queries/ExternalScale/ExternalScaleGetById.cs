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

namespace Greta.BO.BusinessLogic.Handlers.Queries.ExternalScale;

/// <summary>
/// Get entity by id
/// </summary>
/// <param name="Id">Tax id</param>
public record ExternalScaleGetByIdQuery(long Id) : IRequest<ExternalScaleGetByIdResponse>, IAuthorizable, ICacheable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new() {
        new PermissionRequirement.Requirement($"view_external_scale")
    };

    /// <inheritdoc />
    public string CacheKey => $"ExternalScaleGetById{Id}";
}

/// <inheritdoc />
public class ExternalScaleGetByIdHandler : IRequestHandler<ExternalScaleGetByIdQuery, ExternalScaleGetByIdResponse>
{
    private readonly IMapper _mapper;
    private readonly IExternalScaleService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public ExternalScaleGetByIdHandler(IExternalScaleService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<ExternalScaleGetByIdResponse> Handle(ExternalScaleGetByIdQuery request, CancellationToken cancellationToken = default)
    {
        var entity = await _service.Get(request.Id);
        var data = _mapper.Map<ExternalScaleModel>(entity);
        return data == null ? null : new ExternalScaleGetByIdResponse { Data = data };
    }
}

/// <inheritdoc />
public record ExternalScaleGetByIdResponse : CQRSResponse<ExternalScaleModel>;