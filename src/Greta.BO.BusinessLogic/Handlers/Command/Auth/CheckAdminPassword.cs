using System.Threading;
using System.Threading.Tasks;
using Greta.BO.BusinessLogic.Models.Dto;
using MediatR;

namespace Greta.BO.BusinessLogic.Handlers.Command.Auth;

/// <summary>
/// Check Admin Password
/// </summary>
/// <param name="Password"></param>
public record CheckAdminPasswordCommand(string Password) : IRequest<AuthCheckAdminPasswordResponse>;

/// <inheritdoc />
public class CheckAdminPasswordHandler : IRequestHandler<CheckAdminPasswordCommand, AuthCheckAdminPasswordResponse>
{
    /// <inheritdoc />
    public async Task<AuthCheckAdminPasswordResponse> Handle(CheckAdminPasswordCommand request, CancellationToken cancellationToken)
    {
        if (request.Password == null)
        {
            return new AuthCheckAdminPasswordResponse() { Data = false };
        }

        //TODO We can get the password using masstransit from corporate
        var adminPass = "012020";
        return request.Password == adminPass ? new AuthCheckAdminPasswordResponse() { Data = true } : new AuthCheckAdminPasswordResponse() { Data = false };
    }
}

/// <inheritdoc />
public record AuthCheckAdminPasswordResponse : CQRSResponse<bool>;