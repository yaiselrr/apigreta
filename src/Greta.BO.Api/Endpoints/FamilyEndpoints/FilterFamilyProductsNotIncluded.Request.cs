using Greta.BO.BusinessLogic.Models.Dto.Search;
using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.FamilyEndpoints;

public class FilterFamilyProductsNotIncludedRequest
{
    [FromRoute(Name = "currentPage")]public int CurrentPage { get; set; }
    [FromRoute(Name = "pageSize")]public int PageSize { get; set; }
    [FromRoute]public long FamilyId { get; set; }
    [FromBody]public ProductSearchModel Filter { get; set; }
}