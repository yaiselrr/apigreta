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

namespace Greta.BO.BusinessLogic.Handlers.Queries.Device;

/// <summary>
/// Get entity by id
/// </summary>
/// <param name="Id">Device id</param>
public record DeviceGetByIdQuery(long Id) : IRequest<DeviceGetByIdResponse>, IAuthorizable, ICacheable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"view_{nameof(Device).ToLower()}")
    };

    /// <inheritdoc />
    public string CacheKey => $"DeviceGetById{Id}";
}

/// <inheritdoc />
public class DeviceGetByIdHandler : IRequestHandler<DeviceGetByIdQuery, DeviceGetByIdResponse>
{
    private readonly IMapper _mapper;
    private readonly IDeviceService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public DeviceGetByIdHandler(IDeviceService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<DeviceGetByIdResponse> Handle(DeviceGetByIdQuery request, CancellationToken cancellationToken = default)
    {
        var entity = await _service.Get(new GetByIdSpec<Api.Entities.Device>(request.Id), cancellationToken);
        var data = _mapper.Map<DeviceModel>(entity);
        return new DeviceGetByIdResponse { Data = data };
    }
}

/// <inheritdoc />
public record DeviceGetByIdResponse : CQRSResponse<DeviceModel>;