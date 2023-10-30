using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Greta.BO.Api.Abstractions;
using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Service
{
    public interface IPermissionService : IGenericBaseService<Permission>
    {
        Task<List<Permission>> GetByAplication(long applicationId);
    }

    public class PermissionService : BaseService<IPermissionRepository, Permission>, IPermissionService
    {
        public PermissionService(IPermissionRepository repository, ILogger<PermissionService> logger)
            : base(repository, logger)
        {
        }

        /// <summary>
        ///     Get all entities
        /// </summary>
        /// <returns></returns>
        public async Task<List<Permission>> GetByAplication(long applicationId)
        {
            var entities = await _repository.GetEntity<Permission>()
                .Where(x => x.FunctionGroup.ClientApplication.Id == applicationId).ToListAsync();
            return entities;
        }
    }
}