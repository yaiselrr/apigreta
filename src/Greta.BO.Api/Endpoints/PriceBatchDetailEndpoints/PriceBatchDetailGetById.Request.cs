using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.PriceBatchDetailEndpoints;

public class PriceBatchDetailGetByIdRequest
{
    [FromRoute(Name = "entityId")]public int Id { get; set; }
}