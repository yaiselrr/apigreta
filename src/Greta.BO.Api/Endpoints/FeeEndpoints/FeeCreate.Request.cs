using Greta.BO.BusinessLogic.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.FeeEndpoints;

public class FeeCreateRequest
{
    [FromBody] 
    public FeeModel EntityDto { get; set; }
}