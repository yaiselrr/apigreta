using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.VendorOrderEndpoints;

public class VendorOrderGetByIdRequest
{
    [FromRoute(Name = "entityId")]public int Id { get; set; }
}