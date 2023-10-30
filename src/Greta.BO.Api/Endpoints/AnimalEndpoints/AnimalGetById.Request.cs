using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.AnimalEndpoints;

public class AnimalGetByIdRequest
{
    [FromRoute(Name = "entityId")]public int Id { get; set; }
}