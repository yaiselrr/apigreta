using Greta.BO.BusinessLogic.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.InventoryEndpoints;

public class InventoryUpdateOrderAmountRequest
{
    [FromRoute(Name = "storeProductId")] public long Id { get; set; }
    [FromBody] public SuggestedUpdateModel Entity { get; set; }
}