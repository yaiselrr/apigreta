using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.AnimalEndpoints;

public class AnimalDeleteRangeRequest
{
    [FromBody]public List<long> Ids { get; set; }
}