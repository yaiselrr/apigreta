using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.RegionEndpoints;

public class RegionGetByIdRequest
{
    [FromRoute(Name = "entityId")]public int Id { get; set; }
}