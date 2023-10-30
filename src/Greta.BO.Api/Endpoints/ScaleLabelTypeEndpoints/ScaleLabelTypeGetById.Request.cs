using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.ScaleLabelTypeEndpoints;

public class ScaleLabelTypeGetByIdRequest
{
    [FromRoute(Name = "entityId")]public int Id { get; set; }
}