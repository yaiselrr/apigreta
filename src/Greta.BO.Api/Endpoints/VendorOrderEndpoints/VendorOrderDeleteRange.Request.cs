using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.VendorOrderEndpoints;

public class VendorOrderDeleteRangeRequest
{
    [FromBody]public List<long> Ids { get; set; }
}