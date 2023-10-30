using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.FamilyEndpoints;

public class DeleteRangeFamilyProductsRequest
{
    [FromRoute(Name = "entityId")]public long EntityId { get; set; }
    [FromBody]public List<long> Ids { get; set; }
}