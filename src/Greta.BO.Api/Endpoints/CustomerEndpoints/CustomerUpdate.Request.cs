using Greta.BO.Api.Dto;
using Greta.BO.BusinessLogic.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.CustomerEndpoints;

public class CustomerUpdateRequest
{
    [FromRoute(Name = "entityId")]
    public long Id { get; set; }
    [FromBody] 
    public CustomerModel EntityDto { get; set; }
}