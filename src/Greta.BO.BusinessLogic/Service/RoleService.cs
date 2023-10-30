using Greta.BO.Api.Abstractions;
using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Interfaces;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Service
{
    ///<inheritdoc/>
    public interface IRoleService : IGenericBaseService<Role>
    {       
    }


    /// <inheritdoc cref="IRoleService" />
    public class RoleService : BaseService<IRoleRepository, Role>, IRoleService
    {
        ///<inheritdoc/>
        public RoleService(IRoleRepository repository, ILogger<RoleService> logger)
            : base(repository, logger)
        {
        }        
    }
}