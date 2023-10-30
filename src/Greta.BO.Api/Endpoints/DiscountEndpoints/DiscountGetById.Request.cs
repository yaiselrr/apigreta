using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.DiscountEndpoints;

public class DiscountGetByIdRequest
{
    [FromRoute(Name = "entityId")]public int Id { get; set; }
}