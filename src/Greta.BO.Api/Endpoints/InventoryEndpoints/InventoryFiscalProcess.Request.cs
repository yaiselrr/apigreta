using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Models.Dto.Search;
using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.InventoryEndpoints;

public class InventoryFiscalProcessRequest
{    
    [FromBody] public InventoryFiscalModel Filter { get; set; }
}