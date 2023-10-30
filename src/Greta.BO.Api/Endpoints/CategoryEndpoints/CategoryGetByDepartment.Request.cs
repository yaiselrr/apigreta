using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.CategoryEndpoints;

public class CategoryGetByDepartmentRequest
{
    [FromRoute(Name = "departmentId")]public int DepartmentId { get; set; }
}