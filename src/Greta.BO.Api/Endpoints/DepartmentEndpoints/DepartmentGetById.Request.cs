using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.DepartmentEndpoints;

public class DepartmentGetByIdRequest
{
    [FromRoute(Name = "entityId")]public int Id { get; set; }
}