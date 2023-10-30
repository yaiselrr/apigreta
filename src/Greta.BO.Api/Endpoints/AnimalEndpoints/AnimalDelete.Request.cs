using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.AnimalEndpoints;

public class AnimalDeleteRequest
{
    [FromRoute(Name = "entityId")]public int Id { get; set; }
}