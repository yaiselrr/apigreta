using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.CutListDetailEndpoints;

public class CutListDetailDeleteRequest
{
    [FromRoute(Name = "entityId")]public int Id { get; set; }
}