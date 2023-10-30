using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using Greta.Sender.Masstransit.Helper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Command.VendorOrderDetailCredit;

public record VendorOrderInvoiceCreditSendEmailCommand
    (long VendorOrderId) : IRequest<VendorOrderInvoiceCreditSendEmailResponse>, IAuthorizable
{
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"add_edit_vendor_order")
    };
}

public record VendorOrderInvoiceCreditSendEmailResponse : CQRSResponse<bool>;

public class VendorOrderInvoiceCreditSendEmailHandler : IRequestHandler<VendorOrderInvoiceCreditSendEmailCommand,
    VendorOrderInvoiceCreditSendEmailResponse>
{
    private readonly ILogger<VendorOrderInvoiceCreditSendEmailHandler> _logguer;
    private readonly IMediator _mediator;
    private readonly IVendorOrderService _service;
    private readonly IVendorOrderDetailCreditService _serviceDetailCredit;
    private readonly IEmailHelper _emailHelper;

    public VendorOrderInvoiceCreditSendEmailHandler(
        ILogger<VendorOrderInvoiceCreditSendEmailHandler> logguer,
        IMediator mediator,
        IVendorOrderService service,
        IVendorOrderDetailCreditService serviceDetailCredit,
        IEmailHelper emailHelper
    )
    {
        _logguer = logguer;
        _mediator = mediator;
        _service = service;
        _serviceDetailCredit = serviceDetailCredit;
        _emailHelper = emailHelper;
    }

    public async Task<VendorOrderInvoiceCreditSendEmailResponse> Handle(
        VendorOrderInvoiceCreditSendEmailCommand request, CancellationToken cancellationToken)
    {
        _logguer.LogInformation("Preparing credits of vendor order {RequestVendorOrderId} to send order to vendor",
            request.VendorOrderId);
        var credits = await _serviceDetailCredit.GetCreditsByVendorOrder(request.VendorOrderId);

        var order = await _service.GetFullOrder(request.VendorOrderId);

        if (order == null)
        {
            throw new BusinessLogicException("Order not exists.");
        }

        if (credits == null || credits.Count == 0)
        {
            throw new BusinessLogicException("Order without credits.");
        }

        string emailToSend = null;

        if (order.Vendor.VendorContacts.Count > 0)
        {
            var primary = order.Vendor.VendorContacts.FirstOrDefault(x => x.Primary);
            emailToSend = primary == null ? order.Vendor.VendorContacts[0].Email : primary.Email;
        }

        if (emailToSend == null)
        {
            throw new BusinessLogicException("Vendor contact not found.");
        }

        //call mediator csv creator
        //if (order.AttachmentFilePath == null)
        //{
        _logguer.LogInformation("Preparing credits of vendor order {RequestVendorOrderId} file attachment",
            request.VendorOrderId);

        var rPath = await _mediator.Send(new VendorOrderDetailCreditToCsvCommand($"{order.Store.Name}",
            credits), cancellationToken);

        if (rPath.Data == null)
        {
            throw new BusinessLogicException("Csv File can't create.");
        }

        byte[] asBytes = await File.ReadAllBytesAsync(rPath.Data, cancellationToken);
        String asBase64String = Convert.ToBase64String(asBytes);

        //order.AttachmentFilePath = asBase64String;
        //}

        // await _service.Put(request.vendorOrderId, order);
        _logguer.LogInformation("Sending email to {EmailToSend}", emailToSend);

        try
        {
            var resultEmailSender = await _emailHelper.SendVendorOrderCreditsEmail(
                emailToSend,
                order.Store.Name,
                order.Vendor.AccountNumber,
                order.ReceivedDate,
                order.Store.Address,
                order.Store.Phone,
                asBase64String, 
                cancellationToken
            );

            if (resultEmailSender.Status)
            {
                _logguer.LogInformation("Email to {emailToSend} sent.", emailToSend);
            }
            else
            {
                _logguer.LogInformation("Email to {emailToSend} fail. {Info}", emailToSend, resultEmailSender.Message);
            }
        }
        catch (Exception e)
        {
            _logguer.LogError(e, "Email to {emailToSend} fail.", emailToSend);

            throw new BusinessLogicException("Email service unavailable, try again on 1 minute.");
            //return new Response(){ Data = false };
        }

        return new VendorOrderInvoiceCreditSendEmailResponse() { Data = true };
    }
}