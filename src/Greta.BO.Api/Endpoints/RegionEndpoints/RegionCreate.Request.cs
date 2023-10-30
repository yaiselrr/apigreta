using Greta.BO.Api.Dto;
using Greta.BO.BusinessLogic.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.RegionEndpoints;

public class RegionCreateRequest
{
    [FromBody] 
    public RegionModel EntityDto { get; set; }
}