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

namespace Greta.BO.BusinessLogic.Handlers.Queries.Store;

/// <summary>
/// Get entity by id
/// </summary>
/// <param name="Id">Store id</param>
public record StoreGetByIdQuery(long Id) : IRequest<StoreGetByIdResponse>, IAuthorizable, ICacheable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"view_{nameof(Store).ToLower()}")
    };

    /// <inheritdoc />
    public string CacheKey => $"StoreGetById{Id}";
}

/// <inheritdoc />
public class StoreGetByIdHandler : IRequestHandler<StoreGetByIdQuery, StoreGetByIdResponse>
{
    private readonly IMapper _mapper;
    private readonly IStoreService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public StoreGetByIdHandler(IStoreService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<StoreGetByIdResponse> Handle(StoreGetByIdQuery request, CancellationToken cancellationToken = default)
    {
        var entity = await _service.Get(request.Id);
        var data = _mapper.Map<StoreModel>(entity);
        return data == null ? null : new StoreGetByIdResponse { Data = data };
    }
}

/// <inheritdoc />
public record StoreGetByIdResponse : CQRSResponse<StoreModel>;