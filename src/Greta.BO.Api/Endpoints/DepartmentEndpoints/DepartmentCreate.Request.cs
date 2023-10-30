using Greta.BO.Api.Dto;
using Greta.BO.BusinessLogic.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.DepartmentEndpoints;

public class DepartmentCreateRequest
{
    [FromBody] 
    public DepartmentModel EntityDto { get; set; }
}