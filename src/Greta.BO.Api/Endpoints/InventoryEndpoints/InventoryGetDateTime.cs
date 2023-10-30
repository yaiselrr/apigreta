using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.BusinessLogic.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.InventoryEndpoints;
[Route("api/Inventory")]
public class InventoryGet: EndpointBaseAsync.WithoutRequest.WithActionResult<DateTime>
{
    [HttpGet("GetDateTime")]
    [SwaggerOperation(
        Summary = "Get DateTime of Server",
        Description = "Get DateTime of Server",
        OperationId = "Inventory.GetDateTime",
        Tags = new[] { "Inventory" })
    ]
    [ProducesResponseType(typeof(DateTime), 200)]
    public override Task<ActionResult<DateTime>> HandleAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        return Task.FromResult<ActionResult<DateTime>>(Ok(new CQRSResponse<DateTime>() { Data = DateTime.Now }));
    }
}