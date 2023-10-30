using Greta.BO.Api.Dto;
using Greta.BO.BusinessLogic.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.ScaleLabelTypeEndpoints;

public class ScaleLabelTypeCreateRequest
{
    [FromBody] 
    public ScaleLabelTypeModel EntityDto { get; set; }
}