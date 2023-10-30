using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.ShelfTagEndpoints;

public class ShelfTagAddByCategoryRequest
{
    [FromRoute(Name = "categoryId")]
    public long CategoryId { get; set; }
}