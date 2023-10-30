using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.TaxEndpoints;

public class TaxDeleteRangeRequest
{
    [FromBody]public List<long> Ids { get; set; }
}