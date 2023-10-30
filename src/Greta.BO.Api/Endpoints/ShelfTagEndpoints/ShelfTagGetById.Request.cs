using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.ShelfTagEndpoints;

public class ShelfTagGetByIdRequest
{
    [FromRoute(Name = "entityId")]public int Id { get; set; }
}