using Greta.BO.Api.Dto;
using Greta.BO.BusinessLogic.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.PriceBatchDetailEndpoints;

public class PriceBatchDetailCreateRequest
{
    [FromBody] 
    public PriceBatchDetailModel EntityDto { get; set; }
}