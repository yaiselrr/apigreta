using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.VendorOrderDetailEndpoints;

public class VendorOrderDetailDeleteRequest
{
    [FromRoute(Name = "entityId")]public int Id { get; set; }
}