using Greta.BO.Api.Dto;
using Greta.BO.BusinessLogic.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.CutListEndpoints;

public class CutListCreateRequest
{
    [FromBody] 
    public CutListModel EntityDto { get; set; }
}