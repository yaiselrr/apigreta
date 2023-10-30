using Greta.BO.Api.Dto;
using Greta.BO.BusinessLogic.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.OnlineStoreEndpoints;

public class OnlineStoreUpdateRequest
{
    [FromRoute(Name = "entityId")]
    public long Id { get; set; }

    [FromRoute(Name = "token")]
    public string Token { get; set; }

    [FromBody] 
    public OnlineStoreModel EntityDto { get; set; }
}