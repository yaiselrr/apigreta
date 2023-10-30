using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.ProfilesEndpoints;

public class ProfilesGetByIdRequest
{
    [FromRoute(Name = "entityId")]public long Id { get; set; }
}