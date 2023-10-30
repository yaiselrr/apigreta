using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.VendorOrderDetailEndpoints;

public class VendorOrderDetailDeleteRangeRequest
{
    [FromBody]public List<long> Ids { get; set; }
}