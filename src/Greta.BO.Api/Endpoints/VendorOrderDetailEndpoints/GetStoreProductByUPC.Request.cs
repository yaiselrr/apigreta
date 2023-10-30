using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.VendorOrderDetailEndpoints;

public class GetStoreProductByUPCRequest
{
    [FromRoute(Name = "storeId")]public int StoreId { get; set; }
    [FromRoute(Name = "vendorId")]public int VendorId { get; set; }
    [FromRoute(Name = "upc")]public string UPC { get; set; }
}