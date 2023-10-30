using Greta.BO.BusinessLogic.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.VendorOrderDetailCreditEndpoints;

public class UpdateInvoiceCreditsRequest
{
    [FromRoute(Name = "entityId")] public long Id { get; set; }
    [FromBody] public VendorDetailCreditListModel EntityDto { get; set; }
}