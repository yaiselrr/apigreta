using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Handlers.Command.VendorOrder;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;

namespace Greta.BO.BusinessLogic.Handlers.Command.VendorOrderDetail;


/// <summary>
/// Create a vendor order or full order if is DSD
/// </summary>
/// <param name="Entity"></param>
public record VendorOrderDetailCreateCommand(VendorDetailListModel Entity) : IRequest<VendorOrderDetailCreateResponse>,
    IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"add_edit_vendor_order")
    };
}

/// <inheritdoc />
public class VendorOrderDetailCreateHandler : IRequestHandler<VendorOrderDetailCreateCommand, VendorOrderDetailCreateResponse>
{
    private readonly IMapper _mapper;
    private readonly IVendorOrderDetailService _service;
    private readonly IVendorOrderService _serviceVendorOrder;
    private readonly IMediator _mediator;


    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    /// <param name="serviceVendorOrder"></param>
    /// <param name="mapper"></param>
    /// <param name="mediator"></param>
    public VendorOrderDetailCreateHandler(
        IVendorOrderDetailService service,
        IVendorOrderService serviceVendorOrder,
        IMediator mediator,
        
        IMapper mapper)
    {
        _service = service;
        _serviceVendorOrder = serviceVendorOrder;
        _mediator = mediator;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<VendorOrderDetailCreateResponse> Handle(VendorOrderDetailCreateCommand request,
        CancellationToken cancellationToken)
    {
        var vendorOrder = await _serviceVendorOrder.GetWithDetails(request.Entity.VendorOrder);
        var tempDetails = vendorOrder.VendorOrderDetails;
        vendorOrder.VendorOrderDetails = new List<Api.Entities.VendorOrderDetail>();

        var entities = _mapper.Map<List<Api.Entities.VendorOrderDetail>>(request.Entity.Elements);
        if (vendorOrder.IsDirectStoreDelivery)
            foreach (var ent in entities)
            {
                ent.RecivedAmount = ent.OrderAmount;
            }
        var result = await _service.PostMultiple(entities, cancellationToken);

        if (!result)
        {
            vendorOrder.VendorOrderDetails = tempDetails;
            await _serviceVendorOrder.Put(vendorOrder.Id, vendorOrder);
        }

        if (vendorOrder.IsDirectStoreDelivery)
        {
            await _mediator.Send(new VendorOrderReceivedCommand(vendorOrder.Id, new VendorDetailReceivedListModel()
            {
                Elements = await _serviceVendorOrder.GetOnlyReceivedModeDetails(vendorOrder.Id)
            }), cancellationToken);
        }
        return new VendorOrderDetailCreateResponse { Data = result };
    }
}

/// <inheritdoc />
public record VendorOrderDetailCreateResponse : CQRSResponse<bool>;