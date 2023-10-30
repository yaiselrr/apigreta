using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Greta.Sdk.EFCore.Middleware;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Authorization.Requirements;

/// <inheritdoc />
[ExcludeFromCodeCoverage]

public record OnlyConnectedUserRequirementRequirement(string UserId) : IRequirement;

/// <inheritdoc />
public class OnlyConnectedUserRequirementHandler : IRequirementHandler<OnlyConnectedUserRequirementRequirement>
{
    private readonly ILogger<OnlyConnectedUserRequirementHandler> _logger;
    private readonly IAuthenticateUser<string> _authenticateUser;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="authenticateUser"></param>
    public OnlyConnectedUserRequirementHandler(
        ILogger<OnlyConnectedUserRequirementHandler> logger,
        IAuthenticateUser<string> authenticateUser
    )
    {
        this._logger = logger;
        this._authenticateUser = authenticateUser;
    }

    /// <inheritdoc />
    public Task<AuthorizationResult> Handle(OnlyConnectedUserRequirementRequirement requirement)
    {
        if (!_authenticateUser.IsAuthenticated)
        {
            _logger.LogError("User not logged");
            return Task.FromResult(AuthorizationResult.UnauthorizedElem); //("User not logged");
        }

        if (_authenticateUser.UserId == requirement.UserId)
        {
            _logger.LogError("only authenticate user can access to this action");
            return Task.FromResult(AuthorizationResult.UnauthorizedElem); //("User not logged");
        }

        _logger.LogInformation("User {UserName} has access", _authenticateUser.UserName);
        return Task.FromResult(AuthorizationResult.Authorized);
    }
}