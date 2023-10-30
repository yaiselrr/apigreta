using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.OnlineStoreEndpoints;

public class OnlineStoreDeleteRangeRequest
{
    [FromBody]public List<long> Ids { get; set; }
}