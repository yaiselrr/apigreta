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
/// Get entity by storeId
/// </summary>
/// <param name="StoreId">Tax id</param>
public record ExternalScaleGetByStoreQuery(long StoreId) : IRequest<ExternalScaleGetByStoreResponse>, IAuthorizable, ICacheable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new() {
        new PermissionRequirement.Requirement($"view_external_scale")
    };

    /// <inheritdoc />
    public string CacheKey => $"ExternalScaleGetByStore{StoreId}";
}

/// <inheritdoc />
public class ExternalScaleGetByStoreHandler : IRequestHandler<ExternalScaleGetByStoreQuery, ExternalScaleGetByStoreResponse>
{
    private readonly IMapper _mapper;
    private readonly IExternalScaleService _service;

    /// <summary>
    /// Get entity by storeId
    /// </summary>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public ExternalScaleGetByStoreHandler(IExternalScaleService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<ExternalScaleGetByStoreResponse> Handle(ExternalScaleGetByStoreQuery request, CancellationToken cancellationToken = default)
    {
        var entities = await _service.GetExternalScaleByStore(request.StoreId);
        return new ExternalScaleGetByStoreResponse { Data = _mapper.Map<List<ExternalScaleModel>>(entities) };
    }
}

/// <inheritdoc />
public record ExternalScaleGetByStoreResponse : CQRSResponse<List<ExternalScaleModel>>;