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

namespace Greta.BO.BusinessLogic.Handlers.Command.VendorOrder;

/// <inheritdoc />
public record VendorOrderUpdateReceiveCommand
    (long Id, VendorOrderReceiveModel Entity) : IRequest<VendorOrderUpdateReceiveResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"add_edit_vendor_order")
    };
}

/// <inheritdoc />
public class VendorOrderUpdateReceiveHandler : IRequestHandler<VendorOrderUpdateReceiveCommand, VendorOrderUpdateReceiveResponse>
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
    public VendorOrderUpdateReceiveHandler(
        ILogger<VendorOrderUpdateReceiveHandler> logger,
        IVendorOrderService service,
        IMapper mapper)
    {
        _logger = logger;
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<VendorOrderUpdateReceiveResponse> Handle(VendorOrderUpdateReceiveCommand request,
        CancellationToken cancellationToken)
    {
        var vendorOrder = await _service.Get(request.Id);
        vendorOrder.InvoiceNumber = request.Entity.InvoiceNumber;
        vendorOrder.DeliveryCharge = request.Entity.DeliveryCharge;
        var success = await _service.Put(request.Id, vendorOrder);
        return new VendorOrderUpdateReceiveResponse { Data = success };
    }
}

/// <inheritdoc />
public record VendorOrderUpdateReceiveResponse : CQRSResponse<bool>;