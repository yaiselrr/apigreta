using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.FeeEndpoints;

public class FeeDeleteRequest
{
    [FromRoute(Name = "entityId")]public int Id { get; set; }
}