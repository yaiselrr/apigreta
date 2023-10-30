using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.VendorOrderDetailEndpoints;

public class GetStatusPurchaseOrderRequest
{
    [FromRoute(Name = "vendorOrderId")]public int Id { get; set; }
}
