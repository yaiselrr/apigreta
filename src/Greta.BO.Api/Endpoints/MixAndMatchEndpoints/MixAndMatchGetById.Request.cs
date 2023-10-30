using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.MixAndMatchEndpoints;

public class MixAndMatchGetByIdRequest
{
    [FromRoute(Name = "entityId")]public int Id { get; set; }
}