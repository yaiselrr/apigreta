using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.RoleEndpoints;

public class RoleDeleteRequest
{
    [FromRoute(Name = "entityId")]public int Id { get; set; }
}