using System.Collections.Generic;
using Greta.BO.BusinessLogic.Authorization.Requirements;

namespace Greta.BO.BusinessLogic.Authorization
{
    /// <summary>
    /// define one handler need to 
    /// </summary>
    public interface IAuthorizable
    {
        /// <summary>
        /// List of base or custom requirement
        /// </summary>
        List<IRequirement> Requirements { get; }
    }
}