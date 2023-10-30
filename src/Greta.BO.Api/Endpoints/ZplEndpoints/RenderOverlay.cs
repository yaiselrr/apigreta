using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.ExternalClients;
using Greta.BO.BusinessLogic.Handlers.Command.Zpl;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.Sdk.LabelConverter.models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.ZplEndpoints;
[Route("api/Zpl")]
public class RenderOverlay: EndpointBaseAsync.WithRequest<ToZplExmpleRequest>.WithActionResult<RenderZplImageModel>
{
    private readonly IMediator _mediator;

    public RenderOverlay(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("RenderOverlay")]
    [SwaggerOperation(
        Summary = "Get image from labeary using the client.",
        Description = "Get image from labeary using the client.",
        OperationId = "Zpl.RenderOverlay",
        Tags = new[] { "Zpl" })
    ]
    [ProducesResponseType(typeof(RenderZplImageModel), 200)]
    public override async Task<ActionResult<RenderZplImageModel>> HandleAsync(ToZplExmpleRequest request, CancellationToken cancellationToken = new CancellationToken())
    {
        var label = JsonConvert.DeserializeObject<LabelDesign>(request.Design);

        var zpl =(await _mediator.Send(new ConvertZplForExampleCommand(label, request.Type), cancellationToken)).Data;

        var labelaryClient = new LabelaryClient();
        var labelaryImageData = await labelaryClient.GetPreviewAsync(
            zpl,
            ConvertLabelaryDensity(label.dpi == 300 ? 12 : 8),
            new LabelaryClient.LabelSize(label.width / label.dpi, label.height / label.dpi, LabelaryClient.Measure.Inch)
        );

        return new RenderZplImageModel() { Data = Convert.ToBase64String(labelaryImageData) };
    }
    
    private static LabelaryClient.PrintDensity ConvertLabelaryDensity(int density)
    {
        return density switch
        {
            6 => LabelaryClient.PrintDensity.PD6dpmm,
            8 => LabelaryClient.PrintDensity.PD8dpmm,
            12 => LabelaryClient.PrintDensity.PD12dpmm,
            24 => LabelaryClient.PrintDensity.PD24dpmm,
            _ => throw new NotSupportedException($"Print density '{density}' is not supported by labelary."),
        };
    }
}