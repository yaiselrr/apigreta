using System.Threading.Tasks;

namespace Greta.BO.BusinessLogic.Authorization.Requirements
{
    public interface IRequirementHandler
    {
    }

    public interface IRequirementHandler<T> : IRequirementHandler where T : IRequirement
    {
        Task<AuthorizationResult> Handle(T requirement);
    }
}