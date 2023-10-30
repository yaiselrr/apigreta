using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Greta.BO.BusinessLogic.Handlers.Queries.Auth;

namespace Greta.BO.Api.Endpoints.AuthEndpoints;
[Route("api/Auth")]
public class AuthGetUserInfoByUserId : EndpointBaseAsync.WithRequest<AuthGetUserInfoByUserIdRequest>.WithActionResult<AuthUserInfoResponse>
{
    private readonly IMediator _mediator;

    public AuthGetUserInfoByUserId(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("GetUserInfo/{userId}")]
    [SwaggerOperation(
        Summary = "Get user info auth",
        Description = "Get user info auth",
        OperationId = "Auth.GetUserInfo",
        Tags = new[] { "Auth" })
    ]
    [ProducesResponseType(typeof(AuthUserInfoResponse), 200)]
    public override async Task<ActionResult<AuthUserInfoResponse>> HandleAsync(
        [FromRoute] AuthGetUserInfoByUserIdRequest request,
        CancellationToken cancellationToken = default)
    {
        return Ok(await _mediator.Send(new AuthUserInfoQuery(request.UserId), cancellationToken));
    }
}