using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.FamilyEndpoints;

public class FamilyDeleteRequest
{
    [FromRoute(Name = "entityId")]public int Id { get; set; }
}