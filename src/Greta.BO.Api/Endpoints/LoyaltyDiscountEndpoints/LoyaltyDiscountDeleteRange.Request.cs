using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.LoyaltyDiscountEndpoints;

public class LoyaltyDiscountDeleteRangeRequest
{
    [FromBody]public List<long> Ids { get; set; }
}