using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.ExternalScaleEndpoints;

public class ExternalScaleDeleteRangeRequest
{
    [FromBody]public List<long> Ids { get; set; }
}