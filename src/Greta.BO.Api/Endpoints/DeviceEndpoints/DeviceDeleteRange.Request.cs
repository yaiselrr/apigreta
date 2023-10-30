using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.DeviceEndpoints;

public class DeviceDeleteRangeRequest
{
    [FromBody]public List<long> Ids { get; set; }
}