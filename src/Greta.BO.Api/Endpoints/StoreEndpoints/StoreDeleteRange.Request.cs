using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.StoreEndpoints;

public class StoreDeleteRangeRequest
{
    [FromBody]public List<long> Ids { get; set; }
}