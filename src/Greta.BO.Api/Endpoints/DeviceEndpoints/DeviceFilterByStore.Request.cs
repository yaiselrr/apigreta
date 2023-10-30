using Greta.BO.BusinessLogic.Models.Dto.Search;
using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.DeviceEndpoints;

public class DeviceFilterByStoreRequest
{
    [FromRoute(Name = "storeId")]public long Storeid { get; set; }
    [FromRoute(Name = "currentPage")]public int CurrentPage { get; set; }
    [FromRoute(Name = "pageSize")]public int PageSize { get; set; }
    [FromBody]public DeviceSearchModel Filter { get; set; }
}