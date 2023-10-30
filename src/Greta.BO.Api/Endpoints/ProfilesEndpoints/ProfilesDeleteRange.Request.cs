using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.ProfilesEndpoints;

public class ProfilesDeleteRangeRequest
{
    [FromBody]public List<long> Ids { get; set; }
}