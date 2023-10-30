using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using Greta.Sender.Masstransit.Helper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Handlers.Command.VendorOrder;

public record VendorOrderSendEmailCommand(long VendorOrderId) : IRequest<VendorOrderSendEmailResponse>, IAuthorizable
{
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"add_edit_vendor_order")
    };
}

/// <inheritdoc />
public record VendorOrderSendEmailResponse : CQRSResponse<bool>;

/// <inheritdoc />
public class VendorOrderSendEmailHandler : IRequestHandler<VendorOrderSendEmailCommand, VendorOrderSendEmailResponse>
{
    private readonly ILogger<VendorOrderSendEmailHandler> _logguer;
    private readonly IMediator _mediator;
    private readonly IVendorOrderService _service;
    private readonly IEmailHelper _emailHelper;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logguer"></param>
    /// <param name="mediator"></param>
    /// <param name="service"></param>
    /// <param name="emailHelper"></param>
    public VendorOrderSendEmailHandler(
        ILogger<VendorOrderSendEmailHandler> logguer,
        IMediator mediator,
        IVendorOrderService service,
        IEmailHelper emailHelper
    )
    {
        _logguer = logguer;
        _mediator = mediator;
        _service = service;
        _emailHelper = emailHelper;
    }

    /// <inheritdoc />
    public async Task<VendorOrderSendEmailResponse> Handle(VendorOrderSendEmailCommand request,
        CancellationToken cancellationToken)
    {
        _logguer.LogInformation("Preparing vendor order {RequestVendorOrderId} to send order to vendor",
            request.VendorOrderId);
        var order = await _service.GetFullOrder(request.VendorOrderId);

        if (order.VendorOrderDetails == null || order.VendorOrderDetails.Count == 0)
        {
            throw new BusinessLogicException("Order without products.");
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

        order.Status = VendorOrderStatus.Preparing;
        await _service.Put(request.VendorOrderId, order);


        //call mediator csv creator
        if (order.AttachmentFilePath == null)
        {
            _logguer.LogInformation("Preparing vendor order {RequestVendorOrderId} file attachment",
                request.VendorOrderId);

            var rPath = await _mediator.Send(new VendorOrderToCsvCommand($"{order.Store.Name}",
                order.VendorOrderDetails));

            if (rPath.Data == null)
            {
                order.Status = VendorOrderStatus.Failed;
                await _service.Put(request.VendorOrderId, order);
                throw new BusinessLogicException("Csv File can't create.");
            }

            byte[] asBytes = await File.ReadAllBytesAsync(rPath.Data, cancellationToken);
            String asBase64String = Convert.ToBase64String(asBytes);

            order.AttachmentFilePath = asBase64String;
        }

        // await _service.Put(request.vendorOrderId, order);
        _logguer.LogInformation("Sending email to {emailToSend}.", emailToSend);

        try
        {
            var resultEmailSender = await _emailHelper.SendVendorOrderEmail(
                emailToSend,
                order.Store.Name,
                order.Vendor.AccountNumber,
                order.ReceivedDate,
                order.Store.Address,
                order.Store.Phone,
                order.AttachmentFilePath,
                cancellationToken
            );
            if (resultEmailSender.Status)
            {
                _logguer.LogInformation("Email to {emailToSend} sent.", emailToSend);
                if (order.OrderedDate == null)
                    order.OrderedDate = DateTime.UtcNow;
                order.Status = VendorOrderStatus.Sent;
            }
            else
            {
                _logguer.LogInformation("Email to {emailToSend} fail. {Info}", emailToSend, resultEmailSender.Message);
                order.Status = VendorOrderStatus.Failed;
            }

            await _service.Put(request.VendorOrderId, order);
        }
        catch (Exception e)
        {
            _logguer.LogError(e, "Email to {emailToSend} fail.", emailToSend);
            order.Status = VendorOrderStatus.Failed;
            await _service.Put(request.VendorOrderId, order);
            throw new BusinessLogicException("Email service unavailable, try again on 1 minute.");
        }

        return new VendorOrderSendEmailResponse() { Data = true };
    }
}