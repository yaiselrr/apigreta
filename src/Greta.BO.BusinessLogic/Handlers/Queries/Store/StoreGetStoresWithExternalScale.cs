using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;

namespace Greta.BO.BusinessLogic.Handlers.Queries.Store;

/// <summary>
/// Get with external scale entities
/// </summary>
public record StoreGetWithExternalScaleQuery : IRequest<StoreGetWithExternalScaleResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"view_{nameof(Store).ToLower()}")
    };
}

/// <inheritdoc />
public class StoreGetWithExternalScaleHandler : IRequestHandler<StoreGetWithExternalScaleQuery, StoreGetWithExternalScaleResponse>
{
    private readonly IMapper _mapper;
    private readonly IStoreService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public StoreGetWithExternalScaleHandler(IStoreService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<StoreGetWithExternalScaleResponse> Handle(StoreGetWithExternalScaleQuery request, CancellationToken cancellationToken = default)
    {
        var entities = await _service.GetStoresWithExternalScales();
        return new StoreGetWithExternalScaleResponse { Data = _mapper.Map<List<StoreModel>>(entities) };
    }
}

/// <inheritdoc />
public record StoreGetWithExternalScaleResponse : CQRSResponse<List<StoreModel>>;