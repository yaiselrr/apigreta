using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.DepartmentEndpoints;

public class DepartmentDeleteRequest
{
    [FromRoute(Name = "entityId")]public int Id { get; set; }
}