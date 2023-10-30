using Greta.BO.Api.Dto;
using Greta.BO.BusinessLogic.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.VendorOrderEndpoints;

public class VendorOrderCreateRequest
{
    [FromBody] 
    public VendorOrderModel EntityDto { get; set; }
}