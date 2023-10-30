using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.StoreEndpoints;

public class StoreGetByIdRequest
{
    [FromRoute(Name = "entityId")]public int Id { get; set; }
}