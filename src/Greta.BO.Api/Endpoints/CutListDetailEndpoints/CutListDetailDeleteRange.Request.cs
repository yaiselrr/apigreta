using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.CutListDetailEndpoints;

public class CutListDetailDeleteRangeRequest
{
    [FromBody]public List<long> Ids { get; set; }
}