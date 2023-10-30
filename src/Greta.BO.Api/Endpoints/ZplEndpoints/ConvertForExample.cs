using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.BusinessLogic.Handlers.Command.Zpl;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.Sdk.LabelConverter.models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.ZplEndpoints;

[Route("api/Zpl")]
public class ConvertForExample: EndpointBaseAsync.WithRequest<ToZplExmpleRequest>.WithActionResult<ConvertZplForExampleResponse>
{
    private readonly IMediator _mediator;

    public ConvertForExample(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("ConvertForExample")]
    [SwaggerOperation(
        Summary = "Create a zpl code with example data.",
        Description = "Create a zpl code with example data.",
        OperationId = "Zpl.ConvertForExample",
        Tags = new[] { "Zpl" })
    ]
    [ProducesResponseType(typeof(ConvertZplForExampleResponse), 200)]
    public override async Task<ActionResult<ConvertZplForExampleResponse>> HandleAsync(ToZplExmpleRequest request,
        CancellationToken cancellationToken = default)
    {
        var label = JsonConvert.DeserializeObject<LabelDesign>(request.Design);
        return
            Ok(await _mediator.Send(new ConvertZplForExampleCommand(label, request.Type), cancellationToken));
    }
}