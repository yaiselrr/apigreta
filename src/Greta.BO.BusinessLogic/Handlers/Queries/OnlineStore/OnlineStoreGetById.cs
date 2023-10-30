using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Core.Caching;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using Greta.BO.BusinessLogic.Specifications.Generics;
using MediatR;

namespace Greta.BO.BusinessLogic.Handlers.Queries.OnlineStore;

/// <summary>
/// Get entity by id
/// </summary>
/// <param name="Id"></param>
public record OnlineStoreGetByIdQuery(long Id) : IRequest<OnlineStoreGetByIdResponse>, IAuthorizable, ICacheable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"add_edit_{nameof(Store).ToLower()}")
    };

    /// <inheritdoc />
    public string CacheKey => $"OnlineStoreGetById{Id}";
}

/// <inheritdoc />
public class OnlineStoreGetByIdHandler : IRequestHandler<OnlineStoreGetByIdQuery, OnlineStoreGetByIdResponse>
{
    private readonly IMapper _mapper;
    private readonly IOnlineStoreService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public OnlineStoreGetByIdHandler(IOnlineStoreService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<OnlineStoreGetByIdResponse> Handle(OnlineStoreGetByIdQuery request,
        CancellationToken cancellationToken = default)
    {
        var entity = await _service.Get(request.Id);
        var data = _mapper.Map<OnlineStoreModel>(entity);
        return data == null ? null : new OnlineStoreGetByIdResponse { Data = data };
    }
}

/// <inheritdoc />
public record OnlineStoreGetByIdResponse : CQRSResponse<OnlineStoreModel>;