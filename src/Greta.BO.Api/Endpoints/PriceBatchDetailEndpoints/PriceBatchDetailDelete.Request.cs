using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.PriceBatchDetailEndpoints;

public class PriceBatchDetailDeleteRequest
{
    [FromRoute(Name = "entityId")]public int Id { get; set; }
}