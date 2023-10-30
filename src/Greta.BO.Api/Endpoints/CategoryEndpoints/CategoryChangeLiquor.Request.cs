using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.CategoryEndpoints;

public class CategoryChangeLiquorRequest
{
    [FromRoute(Name = "entityId")] public long Id { get; set; }
    [FromRoute(Name = "isLiquorActive")] public bool IsLiquorActive { get; set; }
}