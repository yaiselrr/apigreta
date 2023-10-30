using Greta.BO.BusinessLogic.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.VendorOrderEndpoints;

public class VendorOrderReceivedRequest
{
    [FromRoute(Name = "entityId")] public long Id { get; set; }
    [FromBody] public VendorDetailReceivedListModel EntityDto { get; set; }
}