using Greta.BO.BusinessLogic.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.VendorOrderDetailCreditEndpoints;

public class EditInvoiceCreditRequest
{
    [FromBody] public VendorOrderDetailCreditModel EntityDto { get; set; }
}