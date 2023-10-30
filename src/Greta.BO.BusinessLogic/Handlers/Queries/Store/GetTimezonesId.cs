using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;

namespace Greta.BO.BusinessLogic.Handlers.Queries.Store;

/// <summary>
/// Get time zones id
/// </summary>
public record StoreGetTimezonesIdQuery : IRequest<StoreGetTimezonesIdResponse>
{
}

/// <inheritdoc />
public class StoreGetTimezonesIdHandler : IRequestHandler<StoreGetTimezonesIdQuery, StoreGetTimezonesIdResponse>
{
    private readonly IStoreService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    public StoreGetTimezonesIdHandler(IStoreService service)
    {
        _service = service;
    }

    /// <inheritdoc />
    public async Task<StoreGetTimezonesIdResponse> Handle(StoreGetTimezonesIdQuery request, CancellationToken cancellationToken = default)
    {
        return await Task.FromResult(new StoreGetTimezonesIdResponse() { Data = TimeZoneInfo.GetSystemTimeZones().Select(x => 
                        new TimezoneData(x.Id, $"{x.DisplayName}({x.Id})")).ToList() });
    }    
    
}

/// <inheritdoc />
public record StoreGetTimezonesIdResponse : CQRSResponse<List<TimezoneData>>;
/// <inheritdoc />
public record TimezoneData(string Id, string Name);