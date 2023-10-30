using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.ScaleCategoryEndpoints;

public class ScaleCategoryDeleteRangeRequest
{
    [FromBody]public List<long> Ids { get; set; }
}