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
public class ClockIn : EndpointBaseAsync.WithRequest<ClockInRequest>.WithResult<ClockInResponse>
{
    private readonly IMediator _mediator;

    public ClockIn(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("ClockIn")]
    [SwaggerOperation(
        Summary = "ClockIn",
        Description = "ClockIn",
        OperationId = "TimeKeeping.ClockIn",
        Tags = new[] { "TimeKeeping" })
    ]
    [ProducesResponseType(typeof(ClockInResponse), 200)]
    public override async Task<ClockInResponse> HandleAsync(
        [FromMultiSource] ClockInRequest request, CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new ClockInCommand(
            request.ClockInModel.EmployeeId,
            request.ClockInModel.EmployeeName,
            request.ClockInModel.DeviceLicenceCode,
            request.ClockInModel.Date,
            request.ClockInModel.FormatDate
        ), cancellationToken);
    }
}