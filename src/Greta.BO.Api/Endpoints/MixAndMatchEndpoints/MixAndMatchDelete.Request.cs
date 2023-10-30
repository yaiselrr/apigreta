using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.MixAndMatchEndpoints;

public class MixAndMatchDeleteRequest
{
    [FromRoute(Name = "entityId")]public int Id { get; set; }
}