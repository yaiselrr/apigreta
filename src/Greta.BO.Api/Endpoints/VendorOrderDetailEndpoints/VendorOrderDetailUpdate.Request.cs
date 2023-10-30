using Greta.BO.BusinessLogic.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.VendorOrderDetailEndpoints;

public class VendorOrderDetailUpdateRequest
{
    [FromRoute(Name = "entityId")] public long Id { get; set; }
    [FromBody] public VendorOrderDetailModel EntityDto { get; set; }
}