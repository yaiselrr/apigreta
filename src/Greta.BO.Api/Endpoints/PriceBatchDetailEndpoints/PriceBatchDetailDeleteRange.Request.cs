using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.PriceBatchDetailEndpoints;

public class PriceBatchDetailDeleteRangeRequest
{
    [FromBody]public List<long> Ids { get; set; }
}