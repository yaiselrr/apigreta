using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.RoleEndpoints;

public class RoleGetByIdRequest
{
    [FromRoute(Name = "entityId")]public long Id { get; set; }
}