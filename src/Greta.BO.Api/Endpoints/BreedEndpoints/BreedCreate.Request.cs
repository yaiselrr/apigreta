using Greta.BO.Api.Dto;
using Greta.BO.BusinessLogic.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.BreedEndpoints;

public class BreedCreateRequest
{
    [FromBody] 
    public BreedModel EntityDto { get; set; }
}