using Greta.BO.BusinessLogic.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.VendorOrderDetailCreditEndpoints;

public class SendCreditMailRequest
{
    [FromRoute(Name = "entityId")] public long Id { get; set; }
}