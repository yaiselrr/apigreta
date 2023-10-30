using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.FamilyEndpoints;

public class AddProductsToFamilyRequest
{
    [FromRoute(Name = "entityId")]public long EntityId { get; set; }
    [FromBody]public List<string> Upcs { get; set; }
}