using Greta.BO.BusinessLogic.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.RoleEndpoints;

public class RoleUpdateRequest
{
    [FromRoute(Name = "entityId")]
    public long Id { get; set; }
    [FromBody] 
    public RoleModel EntityDto { get; set; }
}