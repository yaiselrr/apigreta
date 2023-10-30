using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Command.Device;

/// <summary>
/// Change the state of the entity
/// </summary>
/// <param name="Id">Entity Id</param>
/// <param name="State">State to change</param>
public record DeviceChangeStateCommand(long Id, bool State) : IRequest<DeviceChangeStateResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"add_edit_{nameof(Device).ToLower()}")
    };
}

/// <inheritdoc />
public class DeviceChangeStateHandler : IRequestHandler<DeviceChangeStateCommand, DeviceChangeStateResponse>
{
    private readonly ILogger _logger;
    private readonly IDeviceService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    public DeviceChangeStateHandler(ILogger<DeviceChangeStateHandler> logger, IDeviceService service)
    {
        _logger = logger;
        _service = service;
    }

    /// <inheritdoc />
    public async Task<DeviceChangeStateResponse> Handle(DeviceChangeStateCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _service.ChangeState(request.Id, request.State);
        _logger.LogInformation("Entity with {RequestId} state change to {RequestState}", request.Id, request.State);
        return new DeviceChangeStateResponse { Data = result };
    }
}

/// <inheritdoc />
public record DeviceChangeStateResponse : CQRSResponse<bool>;