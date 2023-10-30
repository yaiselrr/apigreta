using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.BreedEndpoints;

public class BreedDeleteRangeRequest
{
    [FromBody]public List<long> Ids { get; set; }
}