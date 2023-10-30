using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.DiscountEndpoints;

public class DiscountDeleteRangeRequest
{
    [FromBody]public List<long> Ids { get; set; }
}