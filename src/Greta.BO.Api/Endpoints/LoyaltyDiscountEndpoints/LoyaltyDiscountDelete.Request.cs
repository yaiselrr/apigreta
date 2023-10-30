using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.LoyaltyDiscountEndpoints;

public class LoyaltyDiscountDeleteRequest
{
    [FromRoute(Name = "entityId")]public int Id { get; set; }
}