using Greta.BO.BusinessLogic.Models.Dto.Search;
using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.DepartmentEndpoints;

public class DepartmentFilterRequest
{
    [FromRoute(Name = "currentPage")]public int CurrentPage { get; set; }
    [FromRoute(Name = "pageSize")]public int PageSize { get; set; }
    [FromBody]public DepartmentSearchModel Filter { get; set; }
}