using Greta.BO.BusinessLogic.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.InventoryEndpoints;

public class InventoryUpdateRequest
{    
    [FromBody] public InventoryUpdateModel Filter { get; set; }
}