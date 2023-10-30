using Greta.BO.BusinessLogic.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.InventoryEndpoints;

public class InventoryFiscalPreProcessRequest
{    
    [FromBody] public InventoryFiscalModel Filter { get; set; }
}