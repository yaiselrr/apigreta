using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.VendorOrderEndpoints;

public class VendorOrderHasProductsRequest
{
    [FromRoute(Name = "vendorOrderId")]public long VendorOrderId { get; set; }
}