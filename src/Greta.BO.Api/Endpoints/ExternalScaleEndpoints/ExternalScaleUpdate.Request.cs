using Greta.BO.Api.Dto;
using Greta.BO.BusinessLogic.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.ExternalScaleEndpoints;

public class ExternalScaleUpdateRequest
{
    [FromRoute(Name = "entityId")]
    public long Id { get; set; }
    [FromBody] 
    public ExternalScaleModel EntityDto { get; set; }
}