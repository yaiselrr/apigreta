using Greta.BO.Api.Entities.Enum;
using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.ScaleLabelTypeEndpoints;

public class ScaleLabelTypeGetByTypeRequest
{
    [FromRoute(Name = "type")]public ScaleType type { get; set; }
}