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

namespace Greta.BO.BusinessLogic.Handlers.Command.VendorOrderDetail;

/// <inheritdoc />
public record VendorOrderDetailAmountUpdateCommand
    (long Id, decimal OrderAmount) : IRequest<VendorOrderDetailAmountUpdateResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"add_edit_vendor_order")
    };
}

/// <inheritdoc />
public class VendorOrderDetailAmountUpdateHandler : IRequestHandler<VendorOrderDetailAmountUpdateCommand, VendorOrderDetailAmountUpdateResponse>
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly IVendorOrderDetailService _service;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    public VendorOrderDetailAmountUpdateHandler(
        ILogger<VendorOrderDetailAmountUpdateHandler> logger,
        IVendorOrderDetailService service,
        IMapper mapper)
    {
        _logger = logger;
        _service = service;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<VendorOrderDetailAmountUpdateResponse> Handle(VendorOrderDetailAmountUpdateCommand request,
        CancellationToken cancellationToken)
    {
        var entity = await _service.Get(request.Id);
        entity.OrderAmount = request.OrderAmount;
        var success = await _service.Put(request.Id, entity);
        return new VendorOrderDetailAmountUpdateResponse { Data = success };
    }
}

/// <inheritdoc />
public record VendorOrderDetailAmountUpdateResponse : CQRSResponse<bool>;