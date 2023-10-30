using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.CutListEndpoints;

public class CutListGetCustomerByAnimalRequest
{
    [FromRoute(Name = "animalId")] public int AnimalId { get; set; }
}