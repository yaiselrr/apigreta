using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.VendorOrderDetailEndpoints;

public class VendorOrderDetailGetByIdRequest
{
    [FromRoute(Name = "entityId")]public int Id { get; set; }
}