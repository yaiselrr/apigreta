using Greta.BO.BusinessLogic.Models.Dto.Search;
using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.InventoryEndpoints;

public class InventoryCreateSuggestedOrderRequest
{
    [FromRoute(Name = "storeId")] public long StoreId { get; set; }
    [FromRoute(Name = "vendorId")] public long VendorId { get; set; }
    [FromBody] public InventorySearchModel Filter { get; set; }
}