using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.CutListEndpoints;

public class CutListGetScaleProductOfTemplateRequest
{
    [FromRoute(Name = "cutListTemplateId")] public long CutListTemplateId { get; set; }
    [FromRoute(Name = "animalId")] public long AnimalId { get; set; }
}