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
public record VendorOrderInvoiceCreditEditCommand
    (VendorOrderDetailCreditModel Entity) : IRequest<VendorOrderInvoiceCreditEditResponse>, IAuthorizable
{
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"add_edit_vendor_order")
    };
}

/// <inheritdoc />
public class VendorOrderInvoiceCreditEditHandler : IRequestHandler<VendorOrderInvoiceCreditEditCommand,
    VendorOrderInvoiceCreditEditResponse>
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
    public VendorOrderInvoiceCreditEditHandler(
        ILogger<VendorOrderInvoiceCreditEditHandler> logger,
        IVendorOrderDetailCreditService serviceDetailCredit,
        IMapper mapper)
    {
        _logger = logger;
        _serviceDetailCredit = serviceDetailCredit;
        _mapper = mapper;
    }

    public async Task<VendorOrderInvoiceCreditEditResponse> Handle(VendorOrderInvoiceCreditEditCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            if (request.Entity != null)
            {
                var elem = request.Entity;
                var orderDetailCredit = await _serviceDetailCredit.Get(new GetByIdSpec<Api.Entities.VendorOrderDetailCredit>(elem.Id), cancellationToken);
                orderDetailCredit.CreditReason = elem.CreditReason;
                orderDetailCredit.CreditQuantity = elem.CreditQuantity;
                orderDetailCredit.IsUnit = elem.IsUnit;
                orderDetailCredit.CreditCost = elem.CreditCost;
                orderDetailCredit.CreditAmount = elem.CreditAmount;

                await _serviceDetailCredit.Put(orderDetailCredit.Id, orderDetailCredit);
            }

            return new VendorOrderInvoiceCreditEditResponse { Data = true };
        }
        catch (Exception ex)
        {
            var t = ex;
            return new VendorOrderInvoiceCreditEditResponse { Data = false };
        }
    }
}

/// <inheritdoc />
public record VendorOrderInvoiceCreditEditResponse : CQRSResponse<bool>;