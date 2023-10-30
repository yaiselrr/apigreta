using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Greta.BO.Api.Attributes;
using Greta.BO.BusinessLogic.Handlers.Command.Auth;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Greta.BO.Api.Endpoints.AuthEndpoints;
[Route("api/Auth")]
public class AuthCheckAdminPassword : EndpointBaseAsync.WithRequest<AuthCheckAdminPasswordRequest>.WithResult<AuthCheckAdminPasswordResponse>
{
    private readonly IMediator _mediator;

    public AuthCheckAdminPassword(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("CheckAdminPassword")]
    [SwaggerOperation(
        Summary = "Check admin password",
        Description = "Check admin password",
        OperationId = "Auth.CheckAdminPassword",
        Tags = new[] { "Auth" })
    ]
    [ProducesResponseType(typeof(AuthCheckAdminPasswordResponse), 200)]
    public override async Task<AuthCheckAdminPasswordResponse> HandleAsync(
        [FromMultiSource] AuthCheckAdminPasswordRequest request,
        CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new CheckAdminPasswordCommand(request.EntityDto.Password), cancellationToken);
    }
}