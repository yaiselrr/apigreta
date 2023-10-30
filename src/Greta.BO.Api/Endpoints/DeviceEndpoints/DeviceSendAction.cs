using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.BusinessLogic.Handlers.Command.Device;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Models.Dto;

namespace Greta.BO.Api.Endpoints.DeviceEndpoints;
[Route("api/Device")]
public class DeviceSendAction : EndpointBaseAsync.WithRequest<DeviceSendActionRequest>.WithActionResult<ActionResponseDeviceModel>
{
    private readonly IMediator _mediator;

    public DeviceSendAction(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("SendActionDevice/{entityId:long}/{actionId:int}")]
    [SwaggerOperation(
        Summary = "Send Action Device",
        Description = "Send Action Device",
        OperationId = "Device.SendActionDevice",
        Tags = new[] { "Device" })
    ]
    [ProducesResponseType(typeof(ActionResponseDeviceModel), 200)]
    public override async Task<ActionResult<ActionResponseDeviceModel>> HandleAsync(
        [FromRoute] DeviceSendActionRequest request,
        CancellationToken cancellationToken = default)
    {
        ActionDeviceType action = (ActionDeviceType)request.ActionId;
        return Ok(action switch
        {
            ActionDeviceType.REBOOT => new CQRSResponse<ActionResponseDeviceModel>()
            {
                Data = new ActionResponseDeviceModel()
                {
                    State = await _mediator.Send(new SendActionCommand(
                        request.EntityId,
                        Command.Reboot,
                        ""
                        ), cancellationToken),
                    Message = ActionDeviceType.REBOOT.ToString()
                }
            },
            ActionDeviceType.CLEARCACHE => new CQRSResponse<ActionResponseDeviceModel>()
            {
                Data = new ActionResponseDeviceModel()
                {
                    State = await _mediator.Send(new SendActionCommand(
                        request.EntityId,
                        Command.CleanCache,
                        ""
                    ), cancellationToken),
                    Message = ActionDeviceType.CLEARCACHE.ToString()
                }
            },
            ActionDeviceType.CLOSESESSION => new CQRSResponse<ActionResponseDeviceModel>()
            {
                Data = new ActionResponseDeviceModel()
                {
                    State = await _mediator.Send(new SendActionCommand(
                        request.EntityId,
                        Command.ForceLogOut,
                        ""
                    ), cancellationToken),
                    Message = ActionDeviceType.CLOSESESSION.ToString()
                }
            },
            ActionDeviceType.UPDATE => new CQRSResponse<ActionResponseDeviceModel>()
            {
                Data = new ActionResponseDeviceModel()
                {
                    State = await _mediator.Send(new SendActionCommand(
                        request.EntityId,
                        Command.ReloadApps,
                        ""
                    ), cancellationToken),
                    Message = ActionDeviceType.UPDATE.ToString()
                }
            },
            ActionDeviceType.CLOSEAPPS => new CQRSResponse<ActionResponseDeviceModel>()
            {
                Data = new ActionResponseDeviceModel()
                {
                    State = await _mediator.Send(new SendActionCommand(
                        request.EntityId,
                        Command.Close,
                        ""
                    ), cancellationToken),
                    Message = ActionDeviceType.OBTAINSTATUS.ToString()
                }
            },
            _ => throw new BussinessValidationException("No action")
        });
    }
}