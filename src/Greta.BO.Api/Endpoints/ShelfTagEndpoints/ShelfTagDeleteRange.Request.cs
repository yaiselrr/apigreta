using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.ShelfTagEndpoints;

public class ShelfTagDeleteRangeRequest
{
    [FromBody]public List<long> Ids { get; set; }
}