using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Greta.BO.Api.Abstractions;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Entities.Lite;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Interfaces;
using Greta.BO.BusinessLogic.Specifications.ProfilesSpecs;
using Greta.Sdk.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Service
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public interface IProfilesService : IGenericBaseService<Profiles>
    { 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        Task<List<Profiles>> GetByApplication(long applicationId);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        Task<bool> ExistWithThisApplication(long id, long applicationId);
    }

    /// <summary>
    /// 
    /// </summary>
    public class ProfilesService : BaseService<IProfilesRepository, Profiles>, IProfilesService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="logger"></param>
        public ProfilesService(IProfilesRepository repository, ILogger<ProfilesService> logger)
        : base(repository, logger)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="logger"></param>
        /// <param name="synchroService"></param>
        public ProfilesService(IProfilesRepository repository, ILogger<ProfilesService> logger,
            ISynchroService synchroService)
            : base(repository, logger, synchroService, Converter)
        {
        }
        private static object Converter(Profiles from) => (LiteProfile.Convert(from));

        /// <summary>
        ///     Get all entities
        /// </summary>
        /// <returns></returns>
        public override async Task<List<Profiles>> Get()
        {
            var entities = await _repository.GetEntity<Profiles>()
                .Include(x => x.Permissions)
                .Include(x => x.Application)
                .ToListAsync();
            return entities;
        }

        /// <summary>
        ///     Get entity by Id
        /// </summary>
        /// <param name = "id"> Id </param>
        /// <returns > Customer </returns>
        /// <exception cref="BusinessLogicException">If id is less to -1</exception>
        public override async Task<Profiles> Get(long id)
        {
            if (id < 1)
            {
                _logger.LogError("Id parameter out of bounds");
                throw new BusinessLogicException("Id parameter out of bounds");
            }

            var spec = new ProfilesGetByIdSpec(id);
            return await _repository.GetEntity<Profiles>().WithSpecification(spec).SingleOrDefaultAsync();
        }

        /// <summary>
        ///     Insert a entity
        /// </summary>
        /// <param name="entity">Entity to insert</param>
        /// <returns>Entity</returns>
        public override async Task<Profiles> Post(Profiles entity)
        {
            var elem1 = new List<Permission>();

            if (entity.Permissions != null)
            {
                var ent = _repository.GetEntity<Permission>();
                for (var i = 0; i < entity.Permissions.Count; i++)
                {
                    var nTax = await ent.FindAsync(entity.Permissions[i].Id);
                    elem1.Add(nTax);
                }
            }

            entity.Permissions = elem1;
            return await base.Post(entity);
        }

        /// <summary>
        ///     Update entity
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="entity">Entity to update</param>
        /// <returns>Boolean success</returns>
        /// <exception cref="BusinessLogicException">If id is less to -1</exception>
        public override async Task<bool> Put(long id, Profiles entity)
        {
            if (id < 1)
            {
                _logger.LogError("Id parameter out of bounds");
                throw new BusinessLogicException("Id parameter out of bounds");
            }
                       
            // remove all stores first
            var profile = await _repository.GetEntity<Profiles>()
                .Include(x => x.Permissions)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
            if (profile == null)
            {
                _logger.LogError("Id parameter out of bounds");
                throw new BusinessLogicException("Id parameter out of bounds");
            }
            var longList = entity.Permissions.Select(x => x.Id).ToList();
            foreach (var store in profile.Permissions.ToList())
                // Remove the roles which are not in the list of new roles
                if (!longList.Contains(store.Id))
                    profile.Permissions.Remove(store);
            foreach (var newStoreId in longList)
                // Add the roles which are not in the list of user's roles
                if (!profile.Permissions.Any(r => r.Id == newStoreId))
                {
                    var newRole = new Permission {Id = newStoreId};
                    _repository.GetEntity<Permission>().Attach(newRole);
                    profile.Permissions.Add(newRole);
                }

            entity.Permissions = profile.Permissions;
           
            if (entity.ApplicationId == 1)
            {
                return await _repository.UpdateAsync(id, entity);
            }
            else
            {
                return await base.Put(id, entity);
            }
        }

        /// <summary>
        ///     Get entity by name
        /// </summary>
        /// <param name="name">Profile name</param>        
        /// <param name="id">Profile id</param>   
        /// <returns>Profiles</returns>
        public async Task<Profiles> GetByName(string name, long id = -1)
        {
            if (id == -1)
            {
                var specByName = new ProfilesGetByNameSpec(name);
                return await _repository.GetEntity<Profiles>().WithSpecification(specByName).FirstOrDefaultAsync();
            }
            var specByNameDistinctId = new ProfilesGetByNameDistinctIdSpec(name, id);
            return await _repository.GetEntity<Profiles>().WithSpecification(specByNameDistinctId).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Check if exist with this Application
        /// </summary>
        /// <param name="id"></param>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        public async Task<bool> ExistWithThisApplication(long id, long applicationId)
        {
            var spec = new ProfilesCheckExistWithThisApplicationSpec(id, applicationId);
            return await _repository.GetEntity<Profiles>().WithSpecification(spec).FirstOrDefaultAsync() == null;
        }


        /// <summary>
        ///     Get entities by application
        /// </summary>
        /// <param name="applicationId">Aplication Id</param>
        /// <returns>Profiles</returns>
        public async Task<List<Profiles>> GetByApplication(long applicationId)
        {
            var spec = new ProfilesGetByApplicationSpec(applicationId);
            return await _repository.GetEntity<Profiles>().WithSpecification(spec).ToListAsync();
        }        
    }
}