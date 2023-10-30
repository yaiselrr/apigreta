using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.ProfilesEndpoints;

public class ProfilesDeleteRequest
{
    [FromRoute(Name = "entityId")]public int Id { get; set; }
}