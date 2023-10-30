using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.RoleEndpoints;

public class RoleDeleteRangeRequest
{
    [FromBody]public List<long> Ids { get; set; }
}