using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.AnimalEndpoints;

public class AnimalGetByRancherRequest
{
    [FromRoute(Name = "rancherId")]public int Id { get; set; }
}