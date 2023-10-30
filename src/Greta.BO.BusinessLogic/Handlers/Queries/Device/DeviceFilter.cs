using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Models.Dto.Search;
using Greta.BO.BusinessLogic.Service;
using Greta.BO.BusinessLogic.Specifications.DeviceSpecs;
using Greta.Sdk.Core.Models.Pager;
using MediatR;

namespace Greta.BO.BusinessLogic.Handlers.Queries.Device;

/// <summary>
/// Query for filter the Device entities
/// </summary>
/// <param name="Storeid">Store id</param>
/// <param name="CurrentPage">Current page</param>
/// <param name="PageSize">Current size</param>
/// <param name="Filter">Filter object of extend of type <see cref="BaseSearchModel"/></param>
public record DeviceFilterQuery
    (long? Storeid, int CurrentPage, int PageSize, DeviceSearchModel Filter) : IRequest<DeviceFilterResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"view_{nameof(Device).ToLower()}")
    };
}

/// <inheritdoc />
public record DeviceFilterResponse : CQRSResponse<Pager<DeviceModel>>;

/// <inheritdoc />
public class DeviceFilterValidator : AbstractValidator<DeviceFilterQuery>
{
    /// <inheritdoc />
    public DeviceFilterValidator()
    {
        RuleFor(x => x.CurrentPage).GreaterThan(0);
        RuleFor(x => x.PageSize).GreaterThan(0);
    }
}

/// <inheritdoc />
public class DeviceFilterHandler : IRequestHandler<DeviceFilterQuery, DeviceFilterResponse>
{
    private readonly IMapper _mapper;
    private readonly IDeviceService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public DeviceFilterHandler(IDeviceService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<DeviceFilterResponse> Handle(DeviceFilterQuery request, CancellationToken cancellationToken = default)
    {
        var entityData = _mapper.Map<DeviceSearchModel>(request.Filter);

        if (request.Storeid.HasValue)
            entityData.StoreId = request.Storeid.Value;
        else
            entityData.StoreId = -1;

        var spec = new DeviceFilterSpec(entityData);

        var data = await _service.FilterSpec(
            request.CurrentPage,
            request.PageSize,
            spec,
            cancellationToken
        );

        return new DeviceFilterResponse { Data = _mapper.Map<Pager<DeviceModel>>(data) };
    }
}