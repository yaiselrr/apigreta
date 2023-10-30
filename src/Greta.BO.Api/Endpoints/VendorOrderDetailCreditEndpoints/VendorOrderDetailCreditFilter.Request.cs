using Greta.BO.BusinessLogic.Models.Dto.Search;
using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.VendorOrderDetailCreditEndpoints;

public class VendorOrderDetailCreditFilterRequest
{
    [FromBody]public VendorOrderDetailCreditSearchModel Filter { get; set; }
}