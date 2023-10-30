using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.FeeEndpoints;

public class FeeDeleteRangeRequest
{
    [FromBody]public List<long> Ids { get; set; }
}