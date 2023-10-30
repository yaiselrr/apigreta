using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Command.PriceBatchDetail;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.IO;

namespace Greta.BO.Api.Endpoints.PriceBatchDetailEndpoints;
[Route("api/PriceBatchDetail")]
public class PriceBatchDetailFileImport : EndpointBaseAsync.WithRequest<PriceBatchDetailFileImportRequest>.WithResult<PriceBatchDetailFileImportResponse>
{
    private readonly IMediator _mediator;

    public PriceBatchDetailFileImport(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("ImportCSV")]
    [SwaggerOperation(
        Summary = "Import a new price batch detail entity",
        Description = "Import a new price batch detail entity",
        OperationId = "PriceBatchDetail.ImportCSV",
        Tags = new[] { "PriceBatchDetail" })
    ]
    [ProducesResponseType(typeof(PriceBatchDetailFileImportResponse), 200)]
    public override async Task<PriceBatchDetailFileImportResponse> HandleAsync(
        [FromMultiSource] PriceBatchDetailFileImportRequest request,
        CancellationToken cancellationToken = default)
    {
        var reader = new StreamReader(request.EntityDto.Csv.OpenReadStream());
        reader.ReadToEnd();

        return await _mediator.Send(new PriceBatchDetailFileImportCommand(request.EntityDto));
    }
}