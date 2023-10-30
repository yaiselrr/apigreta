using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.RegionEndpoints;

public class RegionDeleteRangeRequest
{
    [FromBody]public List<long> Ids { get; set; }
}