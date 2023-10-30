using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.CutListTemplateEndpoints;

public class CutListTemplateDeleteRangeRequest
{
    [FromBody]public List<long> Ids { get; set; }
}