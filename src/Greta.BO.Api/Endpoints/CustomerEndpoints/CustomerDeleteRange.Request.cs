using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.CustomerEndpoints;

public class CustomerDeleteRangeRequest
{
    [FromBody]public List<long> Ids { get; set; }
}