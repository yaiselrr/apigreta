using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.TaxEndpoints;

public class TaxDeleteRequest
{
    [FromRoute(Name = "entityId")]public int Id { get; set; }
}