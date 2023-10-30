using System;
using System.Threading;
using System.Threading.Tasks;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;

namespace Greta.BO.BusinessLogic.Handlers.Queries.Store;

/// <summary>
/// Get entity by guid
/// </summary>
/// <param name="guid">Guid guid</param>
public record StoreGetByGuidQuery(Guid guid) : IRequest<StoreGetByGuidResponse>
{
}

/// <inheritdoc />
public class StoreGetByGuidHandler : IRequestHandler<StoreGetByGuidQuery, StoreGetByGuidResponse>
{
    private readonly IStoreService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    public StoreGetByGuidHandler(IStoreService service)
    {
        _service = service;
    }

    /// <inheritdoc />
    public async Task<StoreGetByGuidResponse> Handle(StoreGetByGuidQuery request, CancellationToken cancellationToken = default)
    {
        var entity = await _service.GetByGuid(request.guid);
        return new StoreGetByGuidResponse {Data = entity};
    }
}

/// <inheritdoc />
public record StoreGetByGuidResponse : CQRSResponse<Api.Entities.Store>;