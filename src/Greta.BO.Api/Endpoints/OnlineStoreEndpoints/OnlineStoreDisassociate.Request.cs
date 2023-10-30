using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.OnlineStoreEndpoints;

public class OnlineStoreDisassociateRequest
{
    // [FromRoute(Name = "entityId")] public long Id { get; set; }
    [FromBody]public List<long> Ids { get; set; }
}