using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.CutListTemplateEndpoints;

public class CutListTemplateGetByIdRequest
{
    [FromRoute(Name = "entityId")]public int Id { get; set; }
}