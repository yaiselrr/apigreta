using System.Diagnostics.CodeAnalysis;

namespace Greta.BO.BusinessLogic.Authorization
{
    [ExcludeFromCodeCoverage]
    public record AuthorizationResult
    {
        public bool IsAuthorized { get; set; }
        public string Message { get; set; }

        public static AuthorizationResult Authorized => new() { IsAuthorized = true };
        public static AuthorizationResult UnauthorizedElem => new() { Message = "User does not have access to perform this action." };

        public static AuthorizationResult UnAuthorized(string message)
        {
            return new() { Message = message };
        }
    }
}