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
/// Get for dashboard all entities
/// </summary>
public record StoreGetForDashboardAllQuery : IRequest<StoreGetForDashboardAllResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"view_{nameof(Store).ToLower()}")
    };
}

/// <inheritdoc />
public class StoreGetForDashboardAllHandler : IRequestHandler<StoreGetForDashboardAllQuery, StoreGetForDashboardAllResponse>
{
    private readonly IMapper _mapper;
    private readonly IStoreService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public StoreGetForDashboardAllHandler(IStoreService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<StoreGetForDashboardAllResponse> Handle(StoreGetForDashboardAllQuery request, CancellationToken cancellationToken = default)
    {
        var entities = await _service.GetForDashboard();
        return new StoreGetForDashboardAllResponse { Data = _mapper.Map<List<StoreModel>>(entities) };
    }
}

/// <inheritdoc />
public record StoreGetForDashboardAllResponse : CQRSResponse<List<StoreModel>>;