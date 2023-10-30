using System;
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

namespace Greta.BO.BusinessLogic.Handlers.Command.VendorOrderDetailCredit;

/// <inheritdoc />
public record VendorOrderInvoiceCreditCommand(long EntityId, VendorDetailCreditListModel Entity) : IRequest<VendorOrderInvoiceCreditResponse>, IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"add_edit_vendor_order")
    };
}

/// <inheritdoc />
public class VendorOrderInvoiceCreditHandler : IRequestHandler<VendorOrderInvoiceCreditCommand, VendorOrderInvoiceCreditResponse>
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly IVendorOrderDetailCreditService _serviceDetailCredit;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="serviceDetailCredit"></param>
    /// <param name="mapper"></param>
    public VendorOrderInvoiceCreditHandler(
        ILogger<VendorOrderInvoiceCreditHandler> logger,
        IVendorOrderDetailCreditService serviceDetailCredit,
        IMapper mapper)
    {
        _logger = logger;
        _serviceDetailCredit = serviceDetailCredit;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<VendorOrderInvoiceCreditResponse> Handle(VendorOrderInvoiceCreditCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            foreach (var elem in request.Entity.Elements)
            {
                var orderDetailCredit = await _serviceDetailCredit.Get(new GetByIdSpec<Api.Entities.VendorOrderDetailCredit>(elem.Id), cancellationToken);;
                orderDetailCredit.CreditReason = elem.CreditReason;
                orderDetailCredit.CreditQuantity = elem.CreditQuantity;
                orderDetailCredit.IsUnit = elem.IsUnit;
                orderDetailCredit.CreditCost = elem.CreditCost;
                orderDetailCredit.CreditAmount = elem.CreditAmount;

                await _serviceDetailCredit.Put(orderDetailCredit.Id, orderDetailCredit);
            }

            return new VendorOrderInvoiceCreditResponse { Data = true };
        }
        catch (Exception ex)
        {
            var t = ex;
            return new VendorOrderInvoiceCreditResponse { Data = false };
        }
    }
}

/// <inheritdoc />
public record VendorOrderInvoiceCreditResponse : CQRSResponse<bool>;