using Greta.BO.BusinessLogic.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.VendorOrderDetailEndpoints;

public class VendorOrderDetailAmountUpdateRequest
{
    [FromRoute(Name = "entityId")] public long Id { get; set; }
    [FromBody] public OrderAmountDto OrderAmount { get; set; }
}

public class OrderAmountDto
{
    public decimal OrderAmount { get; set; }
}