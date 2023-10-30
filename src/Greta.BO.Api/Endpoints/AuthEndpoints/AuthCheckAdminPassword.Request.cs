using Greta.BO.BusinessLogic.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.AuthEndpoints;

public class AuthCheckAdminPasswordRequest
{
    [FromBody] 
    public AdminPasswordModel EntityDto { get; set; }
}