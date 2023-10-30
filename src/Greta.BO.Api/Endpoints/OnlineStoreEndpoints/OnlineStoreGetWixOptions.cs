using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Greta.BO.Wix.Models;

namespace Greta.BO.Api.Endpoints.OnlineStoreEndpoints
{
    [Route("api/OnlineStore/GetWixOptions")]
    public class OnlineStoreGetWixOptions : EndpointBaseAsync.WithoutRequest.WithActionResult<WixOptions>
    {
       protected readonly WixOptions options;

        public OnlineStoreGetWixOptions(WixOptions options)
        {
          this.options = options;
        }

        [HttpGet("")]
        [SwaggerOperation(
            Summary = "Get Options for install Store",
            Description = "Get Options for install Store"
        )]
        [ProducesResponseType(typeof(WixOptions), 200)]
        public override async Task<ActionResult<WixOptions>> HandleAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var wixOptions = new WixOptions
            {
                ClientId = options.ClientId,
                ClientSecret = options.ClientSecret,
                UrlShared = options.UrlShared,
                RedirectUrl = options.RedirectUrl
            };

            return Ok(wixOptions);
        }
    }
}