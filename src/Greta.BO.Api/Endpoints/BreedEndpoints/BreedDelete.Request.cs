using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.BreedEndpoints;

public class BreedDeleteRequest
{
    [FromRoute(Name = "entityId")]public int Id { get; set; }
}