using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.AnimalEndpoints;

public class AnimalGetByBreedRequest
{
    [FromRoute(Name = "breedId")]public int Id { get; set; }
}