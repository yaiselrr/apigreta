using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;
using Microsoft.Extensions.Logging;
using Greta.BO.BusinessLogic.Handlers.Command.DeviceConfiguration;

namespace Greta.BO.BusinessLogic.Handlers.Command.Device;

/// <summary>
/// Update entity
/// </summary>
/// <param name="Id">Entity id</param>
/// <param name="Entity">New entity</param>
public record DeviceUpdateCommand
    (long Id, DeviceModel Entity) : IRequest<DeviceUpdateResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"add_edit_{nameof(Device).ToLower()}")
    };
}

/// <inheritdoc />
public class DeviceUpdateHandler : IRequestHandler<DeviceUpdateCommand, DeviceUpdateResponse>
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly IDeviceService _service;
    private readonly IMediator _mediator;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    /// <param name="mediator"></param>
    /// <param name="mapper"></param>
    public DeviceUpdateHandler(
        ILogger<DeviceUpdateHandler> logger,
                IDeviceService service,
               
                IMediator mediator,
                IMapper mapper)
    {
        _logger = logger;
        _service = service;
        _mediator = mediator;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<DeviceUpdateResponse> Handle(DeviceUpdateCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Api.Entities.Device>(request.Entity);
        var success = await _service.PutConfiguration(request.Id, entity);
        if (success)
        {
            //if the current device is connected then send the new configuration now
            await _mediator.Send(new SendToDeviceCommand(null, request.Id), cancellationToken);
        }
        _logger.LogInformation("Device configuration for device with id {DeviceId} update successfully", request.Id);
        return new DeviceUpdateResponse { Data = success };
    }
}

/// <inheritdoc />
public record DeviceUpdateResponse : CQRSResponse<bool>;