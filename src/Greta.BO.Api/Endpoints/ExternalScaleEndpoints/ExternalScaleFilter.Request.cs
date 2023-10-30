using Greta.BO.BusinessLogic.Models.Dto.Search;
using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.ExternalScaleEndpoints;

public class ExternalScaleFilterRequest
{
    [FromRoute(Name = "currentPage")]public int CurrentPage { get; set; }
    [FromRoute(Name = "pageSize")]public int PageSize { get; set; }
    [FromBody]public ExternalScaleSearchModel Filter { get; set; }
}