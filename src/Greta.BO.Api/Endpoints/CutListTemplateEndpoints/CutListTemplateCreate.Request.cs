using Greta.BO.BusinessLogic.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.CutListTemplateEndpoints;

public class CutListTemplateCreateRequest
{
    [FromBody] 
    public CutListTemplateModel EntityDto { get; set; }
}