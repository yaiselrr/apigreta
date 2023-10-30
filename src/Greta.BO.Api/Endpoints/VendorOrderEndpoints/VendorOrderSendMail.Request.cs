using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.VendorOrderEndpoints;

public class VendorOrderSendMailRequest
{
    [FromRoute(Name = "entityId")] public long Id { get; set; }
}