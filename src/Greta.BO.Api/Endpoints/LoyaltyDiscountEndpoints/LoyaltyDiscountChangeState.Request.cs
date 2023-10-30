using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.LoyaltyDiscountEndpoints
{
    public class LoyaltyDiscountChangeStateRequest
    {
        [FromRoute(Name = "entityId")] public long Id { get; set; }
        [FromRoute(Name = "state")] public bool State { get; set; }
    }
}