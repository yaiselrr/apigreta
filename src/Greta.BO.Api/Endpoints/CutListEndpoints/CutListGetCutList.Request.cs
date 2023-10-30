using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.CutListEndpoints;

public class CutListGetCutListRequest
{
    [FromRoute(Name = "includeDetails")] public bool IncludeDetails { get; set; }
    [FromRoute(Name = "animalId")] public int AnimalId { get; set; }
    [FromRoute(Name = "customerId")] public int CustomerId { get; set; }
}