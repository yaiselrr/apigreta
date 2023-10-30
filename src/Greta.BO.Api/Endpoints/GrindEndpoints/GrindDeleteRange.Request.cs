using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.GrindEndpoints;

public class GrindDeleteRangeRequest
{
    [FromBody]public List<long> Ids { get; set; }
}