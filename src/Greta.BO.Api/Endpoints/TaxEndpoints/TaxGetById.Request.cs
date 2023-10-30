using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.TaxEndpoints;

public class TaxGetByIdRequest
{
    [FromRoute(Name = "entityId")]public int Id { get; set; }
}