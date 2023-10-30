using Greta.BO.BusinessLogic.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.FamilyEndpoints;

public class ProfilesCreateRequest
{
    [FromBody] 
    public ProfilesModel EntityDto { get; set; }
}