using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Command.TimeKeeping;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.TimeKeeping;

[Route("api/TimeKeeping")]
public class ClockOut : EndpointBaseAsync.WithRequest<ClockOutRequest>.WithResult<ClockOutResponse>
{
    private readonly IMediator _mediator;

    public ClockOut(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("ClockOut")]
    [SwaggerOperation(
        Summary = "ClockOut",
        Description = "ClockOut",
        OperationId = "TimeKeeping.ClockOut",
        Tags = new[] { "TimeKeeping" })
    ]
    [ProducesResponseType(typeof(ClockOutResponse), 200)]
    public override async Task<ClockOutResponse> HandleAsync(
        [FromMultiSource] ClockOutRequest request, CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new ClockOutCommand(
            request.ClockOutModel.EmployeeId,
            request.ClockOutModel.EmployeeName,
            request.ClockOutModel.DeviceLicenceCode,
            request.ClockOutModel.Date,
            request.ClockOutModel.FormatDate
        ), cancellationToken);
    }
}