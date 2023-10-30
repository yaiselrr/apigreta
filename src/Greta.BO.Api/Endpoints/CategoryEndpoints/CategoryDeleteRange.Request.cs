using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.CategoryEndpoints;

public class CategoryDeleteRangeRequest
{
    [FromBody]public List<long> Ids { get; set; }
}