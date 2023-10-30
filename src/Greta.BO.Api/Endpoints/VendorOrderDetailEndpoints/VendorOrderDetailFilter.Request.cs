using Greta.BO.BusinessLogic.Models.Dto.Search;
using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.VendorOrderDetailEndpoints;

public class VendorOrderDetailFilterRequest
{
    [FromBody]public VendorOrderDetailSearchModel Filter { get; set; }
}