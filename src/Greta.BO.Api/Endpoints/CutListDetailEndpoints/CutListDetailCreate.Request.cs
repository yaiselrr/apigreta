using Greta.BO.BusinessLogic.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.CutListDetailEndpoints;

public class CutListDetailCreateRequest
{
    [FromBody] 
    public CutListDetailListModel EntityDto { get; set; }
}