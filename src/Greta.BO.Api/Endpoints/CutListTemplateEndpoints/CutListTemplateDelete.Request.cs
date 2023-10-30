using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.CutListTemplateEndpoints;

public class CutListTemplateDeleteRequest
{
    [FromRoute(Name = "entityId")]public int Id { get; set; }
}