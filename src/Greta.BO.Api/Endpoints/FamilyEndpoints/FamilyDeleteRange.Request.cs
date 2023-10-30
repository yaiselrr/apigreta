using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.FamilyEndpoints;

public class FamilyDeleteRangeRequest
{
    [FromBody]public List<long> Ids { get; set; }
}