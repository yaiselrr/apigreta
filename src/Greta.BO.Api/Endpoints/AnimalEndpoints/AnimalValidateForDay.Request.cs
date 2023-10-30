using Greta.BO.Api.Dto;
using Greta.BO.BusinessLogic.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.AnimalEndpoints;

public class AnimalValidateForDayRequest
{
    [FromBody] 
    public ValidateADayModel model { get; set; }
}