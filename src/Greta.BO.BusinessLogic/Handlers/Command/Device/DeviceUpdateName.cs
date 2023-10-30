using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using Greta.BO.BusinessLogic.Specifications.Generics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Command.Device;

/// <summary>
/// Update entity
/// </summary>
/// <param name="Id">Entity id</param>
/// <param name="Entity">New entity</param>
public record DeviceUpdateNameCommand
    (long Id, DeviceModel Entity) : IRequest<DeviceUpdateNameResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"add_edit_{nameof(Device).ToLower()}")
    };
}
/// <inheritdoc />
public class DeviceUpdateValidator : AbstractValidator<DeviceUpdateNameCommand>
{
    private readonly IDeviceService _service;

    /// <inheritdoc />
    public DeviceUpdateValidator(IDeviceService service)
    {
        _service = service;
        
        RuleFor(x => x.Entity.Name)
            .NotEmpty()
            .Length(3, 64)
            .MustAsync(NameUnique).WithMessage("Device name already exists.");
    }

    private async Task<bool> NameUnique(DeviceUpdateNameCommand command, string name, CancellationToken cancellationToken)
    {
        var deviceExist = await _service.Get(new CheckUniqueNameSpec<Api.Entities.Device>(name, command.Id), cancellationToken);
        return deviceExist == null;
    }
}

/// <inheritdoc />
public class DeviceUpdateNameHandler : IRequestHandler<DeviceUpdateNameCommand, DeviceUpdateNameResponse>
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly IDeviceService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public DeviceUpdateNameHandler(
        ILogger<DeviceUpdateNameHandler> logger,
                IDeviceService service,
                IMapper mapper)
    {
        _logger = logger;
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<DeviceUpdateNameResponse> Handle(DeviceUpdateNameCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Api.Entities.Device>(request.Entity);
        var success = await _service.PutName(request.Id, entity);
        _logger.LogInformation("Device {DeviceId} update successfully", request.Id);
        return new DeviceUpdateNameResponse { Data = success };
    }
}

/// <inheritdoc />
public record DeviceUpdateNameResponse : CQRSResponse<bool>;