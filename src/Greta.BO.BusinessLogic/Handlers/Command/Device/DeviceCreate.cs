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
/// Create entity
/// </summary>
/// <param name="Entity"></param>
public record DeviceCreateCommand(DeviceModel Entity) : IRequest<DeviceCreateResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"add_edit_{nameof(Device).ToLower()}")
    };
}

/// <inheritdoc />
public class DeviceCreateValidator : AbstractValidator<DeviceCreateCommand>
{
    private readonly IDeviceService _service;

    /// <inheritdoc />
    public DeviceCreateValidator(IDeviceService service)
    {
        _service = service;
        RuleFor(x => x.Entity.Name)
            .NotEmpty()
            .Length(3, 64)
            .MustAsync(NameUnique).WithMessage("Device name already exists.");
    }

    private async Task<bool> NameUnique(string name, CancellationToken cancellationToken)
    {
        var deviceExist = await _service.Get(new CheckUniqueNameSpec<Api.Entities.Device>(name), cancellationToken);
        return deviceExist == null;
    }
}

/// <inheritdoc />
public class DeviceCreateHandler : IRequestHandler<DeviceCreateCommand, DeviceCreateResponse>
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
    public DeviceCreateHandler(
        ILogger<DeviceCreateHandler> logger,
        IDeviceService service,
        IMapper mapper)
    {
        _logger = logger;
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<DeviceCreateResponse> Handle(DeviceCreateCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Api.Entities.Device>(request.Entity);
        var result = await _service.Post(entity);
        _logger.LogInformation("Create Device {DeviceName} for user {UserId}", result.Name, result.UserCreatorId);
        return new DeviceCreateResponse { Data = _mapper.Map<DeviceModel>(result) };
    }
}

/// <inheritdoc />
public record DeviceCreateResponse : CQRSResponse<DeviceModel>;