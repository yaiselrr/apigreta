using Greta.BO.BusinessLogic.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.RoleEndpoints;

public class RoleCreateRequest
{
    [FromBody] 
    public RoleModel EntityDto { get; set; }
}