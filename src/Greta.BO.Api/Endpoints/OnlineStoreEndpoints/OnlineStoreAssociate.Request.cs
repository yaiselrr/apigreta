using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.OnlineStoreEndpoints;

public class OnlineStoreAssociateRequest
{
    [FromRoute(Name = "entityId")] public long Id { get; set; }
    [FromRoute(Name = "token")] public string Token { get; set; }
    [FromRoute(Name = "isImport")] public bool IsImport { get; set; }
}