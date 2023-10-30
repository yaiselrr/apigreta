using Greta.BO.Api.Dto;
using Greta.BO.BusinessLogic.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.GrindEndpoints;

public class GrindCreateRequest
{
    [FromBody] 
    public GrindModel EntityDto { get; set; }
}