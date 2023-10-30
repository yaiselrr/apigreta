using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.ZplEndpoints;

public class ConvertAnimalRequest
{
    [FromRoute(Name = "tagId")]public long TagId { get; set; }
    [FromRoute(Name = "animalId")]public long AnimalId { get; set; }
}