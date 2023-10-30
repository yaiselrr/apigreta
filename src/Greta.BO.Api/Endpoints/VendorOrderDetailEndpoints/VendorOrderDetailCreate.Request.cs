using Greta.BO.Api.Dto;
using Greta.BO.BusinessLogic.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.VendorOrderDetailEndpoints;

public class VendorOrderDetailCreateRequest
{
    [FromBody] 
    public VendorDetailListModel EntityDto { get; set; }
}