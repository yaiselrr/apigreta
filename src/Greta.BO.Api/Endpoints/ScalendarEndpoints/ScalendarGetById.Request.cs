using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.ScalendarEndpoints;

public class ScalendarGetByIdRequest
{
    [FromRoute(Name = "entityId")]public int Id { get; set; }
}