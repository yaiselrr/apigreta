using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.DepartmentEndpoints;

public class DepartmentDeleteRangeRequest
{
    [FromBody]public List<long> Ids { get; set; }
}