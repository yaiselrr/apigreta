using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.VendorOrderEndpoints;

public class VendorOrderDeleteRequest
{
    [FromRoute(Name = "entityId")]public int Id { get; set; }
}