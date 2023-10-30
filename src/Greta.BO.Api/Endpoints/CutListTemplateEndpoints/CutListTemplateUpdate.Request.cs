using Greta.BO.BusinessLogic.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.CutListTemplateEndpoints;

public class CutListTemplateUpdateRequest
{
    [FromRoute(Name = "entityId")]
    public long Id { get; set; }
    [FromBody] 
    public CutListTemplateModel EntityDto { get; set; }
}