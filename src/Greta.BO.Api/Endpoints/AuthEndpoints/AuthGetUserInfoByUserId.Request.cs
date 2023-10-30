using Microsoft.AspNetCore.Mvc;

namespace Greta.BO.Api.Endpoints.AuthEndpoints;

public class AuthGetUserInfoByUserIdRequest
{
    [FromRoute(Name = "userId")] public string UserId { get; set; }
}