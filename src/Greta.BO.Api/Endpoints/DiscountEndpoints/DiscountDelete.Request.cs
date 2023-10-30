using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.DiscountEndpoints;

public class DiscountDeleteRequest
{
    [FromRoute(Name = "entityId")]public int Id { get; set; }
}