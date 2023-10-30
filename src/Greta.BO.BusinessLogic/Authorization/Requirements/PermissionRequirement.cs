using System.Linq;
using System.Threading.Tasks;
using Greta.BO.BusinessLogic.Service;
using Greta.Sdk.EFCore.Middleware;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Authorization.Requirements
{
    public static class PermissionRequirement
    {
        public record Requirement(string PermissionCode) : IRequirement;

        public class Handler : IRequirementHandler<Requirement>
        {
            private readonly ILogger<Handler> logger;
            private readonly IBOUserService boUserService;
            private readonly IAuthenticateUser<string> authenticateUser;

            public Handler(
                ILogger<Handler> logger,
                IBOUserService boUserService,
                IAuthenticateUser<string> authenticateUser
                )
            {
                this.logger = logger;
                this.boUserService = boUserService;
                this.authenticateUser = authenticateUser;
            }

            public async Task<AuthorizationResult> Handle(Requirement requirement)
            {

                if (!authenticateUser.IsAuthenticated)
                {
                    logger.LogError("User not logged");
                    return AuthorizationResult.UnauthorizedElem;//("User not logged");
                }

                var user = await boUserService.GetByUserId(authenticateUser.UserId);

                if (user == null)
                {
                    logger.LogError("User not found");
                    return AuthorizationResult.UnauthorizedElem;//("User not found");
                }

                if (user.BOProfile.Permissions.Any(x => x.Code == requirement.PermissionCode))
                {
                    logger.LogInformation($"User {authenticateUser.UserName} has permission.");
                    return AuthorizationResult.Authorized;
                }

                logger.LogError("User do not have access");
                return AuthorizationResult.UnauthorizedElem;//("User do not have access");
            }
        }
    }
}