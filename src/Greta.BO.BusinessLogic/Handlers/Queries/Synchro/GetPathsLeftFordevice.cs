using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Greta.BO.BusinessLogic.Service;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Queries.Synchro;

public record GetPathsLeftForDeviceQuery
    (string DeviceId, int SynchroVersion, long StoreId) : IRequest<List<Api.Entities.Synchro>>;

public class GetPathsLeftForDeviceHandler : IRequestHandler<GetPathsLeftForDeviceQuery, List<Api.Entities.Synchro>>
{
    private readonly ISynchroService _service;
    private readonly ILogger<GetPathsLeftForDeviceHandler> _logger;

    public GetPathsLeftForDeviceHandler(ISynchroService service, ILogger<GetPathsLeftForDeviceHandler> logger)
    {
        _service = service;
        _logger = logger;
    }

    public async Task<List<Api.Entities.Synchro>> Handle(GetPathsLeftForDeviceQuery request,
        CancellationToken cancellationToken)
    {
        var list = await _service.GetPathsLeftForDevice(request.DeviceId, request.SynchroVersion, request.StoreId);
        _logger.LogInformation("Found {Count} Synchros", list.Count);
        return list;
    }
}