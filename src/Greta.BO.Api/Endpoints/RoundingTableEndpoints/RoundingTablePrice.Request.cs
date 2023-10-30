using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.RoundingTableEndpoints;

public class RoundingTablePriceRequest
{
    [FromRoute(Name = "price")]public decimal Price { get; set; }
}