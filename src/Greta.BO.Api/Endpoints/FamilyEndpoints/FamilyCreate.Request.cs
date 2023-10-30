using Greta.BO.BusinessLogic.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.FamilyEndpoints;

public class FamilyCreateRequest
{
    [FromBody] 
    public FamilyModel EntityDto { get; set; }
}