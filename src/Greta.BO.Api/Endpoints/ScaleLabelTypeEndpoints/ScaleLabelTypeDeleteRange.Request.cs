using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.ScaleLabelTypeEndpoints;

public class ScaleLabelTypeDeleteRangeRequest
{
    [FromBody]public List<long> Ids { get; set; }
}