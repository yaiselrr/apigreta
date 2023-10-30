using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Models.Dto.Search;
using Greta.BO.BusinessLogic.Service;
using Greta.BO.BusinessLogic.Specifications.StoreSpecs;
using MediatR;

namespace Greta.BO.BusinessLogic.Handlers.Queries.Store;

/// <summary>
/// Get all entities
/// </summary>
public record StoreGetAllQuery : IRequest<StoreGetAllResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"view_{nameof(Store).ToLower()}")
    };
}

/// <inheritdoc />
public class StoreGetAllHandler : IRequestHandler<StoreGetAllQuery, StoreGetAllResponse>
{
    private readonly IMapper _mapper;
    private readonly IStoreService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public StoreGetAllHandler(IStoreService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<StoreGetAllResponse> Handle(StoreGetAllQuery request, CancellationToken cancellationToken = default)
    {
        // var entities = await _service.Get();
        var spec = new StoreFilterSpec(new StoreSearchModel
        {
            IsPagingEnabled = false,
            Sort = "",
            Search = "",
            Name = "",
            RegionId = -1,
            IsDeleted = false
        });
        var entities = await _service.Get(spec, cancellationToken);
        return new StoreGetAllResponse { Data = entities };
    }
}

/// <inheritdoc />
public record StoreGetAllResponse : CQRSResponse<IReadOnlyList<StoreListModel>>;