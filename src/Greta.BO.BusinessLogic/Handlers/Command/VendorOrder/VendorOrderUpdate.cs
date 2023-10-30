using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using Greta.BO.BusinessLogic.Specifications.Generics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Command.VendorOrder;

/// <inheritdoc />
public record VendorOrderUpdateCommand(long Id, VendorOrderModel Entity) : IRequest<VendorOrderUpdateResponse>,
    IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"add_edit_vendor_order")
    };
}

/// <inheritdoc />
public class VendorOrderUpdateHandler : IRequestHandler<VendorOrderUpdateCommand, VendorOrderUpdateResponse>
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly IVendorOrderService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public VendorOrderUpdateHandler(
        ILogger<VendorOrderUpdateHandler> logger,
        IVendorOrderService service,
        IMapper mapper)
    {
        _logger = logger;
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<VendorOrderUpdateResponse> Handle(VendorOrderUpdateCommand request,
        CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Api.Entities.VendorOrder>(request.Entity);
        var real = await _service.Get(new GetByIdSpec<Api.Entities.VendorOrder>(request.Id), cancellationToken);
        entity.UserId = real.UserId;
        var success = await _service.Put(request.Id, entity);
        return new VendorOrderUpdateResponse { Data = success };
    }
}

/// <inheritdoc />
public record VendorOrderUpdateResponse : CQRSResponse<bool>;