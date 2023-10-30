using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.MixAndMatchEndpoints;

public class MixAndMatchDeleteRangeRequest
{
    [FromBody]public List<long> Ids { get; set; }
}