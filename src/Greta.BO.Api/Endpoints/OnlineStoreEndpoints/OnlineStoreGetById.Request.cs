using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.OnlineStoreEndpoints;

public class OnlineStoreGetByIdRequest
{
    [FromRoute(Name = "entityId")]public int Id { get; set; }
}