using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;

namespace Greta.BO.BusinessLogic.Handlers.Queries.Device;

/// <summary>
/// Get all entities
/// </summary>
public record DeviceGetAllQuery : IRequest<DeviceGetAllResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"view_{nameof(Device).ToLower()}")
    };
}

/// <inheritdoc />
public class DeviceGetAllHandler : IRequestHandler<DeviceGetAllQuery, DeviceGetAllResponse>
{
    private readonly IMapper _mapper;
    private readonly IDeviceService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public DeviceGetAllHandler(IDeviceService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<DeviceGetAllResponse> Handle(DeviceGetAllQuery request, CancellationToken cancellationToken = default)
    {
        var entities = await _service.Get();
        return new DeviceGetAllResponse { Data = _mapper.Map<List<DeviceModel>>(entities) };
    }
}

/// <inheritdoc />
public record DeviceGetAllResponse : CQRSResponse<List<DeviceModel>>;