using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Greta.BO.Api.Abstractions;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Entities.Lite;
using Greta.BO.Api.Sqlserver.Repository;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Service
{
    public interface IScaleHomeFavService : IGenericBaseService<ScaleHomeFav>
    {
    }

    public class ScaleHomeFavService : BaseService<IScaleHomeFavRepository, ScaleHomeFav>, IScaleHomeFavService
    {
        public ScaleHomeFavService(IScaleHomeFavRepository repository,
            ISynchroService synchroService,
            ILogger<ScaleHomeFavService> logger)
            : base(repository, logger, synchroService, Converter)
        {
        }

        private static object Converter(ScaleHomeFav from) => (LiteScaleHomeFav.Convert(from));

        /// <summary>
        ///     Get entity by Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Customer</returns>
        /// <exception cref="BusinessLogicException">If id is less to -1</exception>
        public override async Task<ScaleHomeFav> Get(long id)
        {
            if (id < 1)
            {
                _logger.LogError("Id parameter out of bounds.");
                throw new BusinessLogicException("Id parameter out of bounds.");
            }

            var entity = await _repository.GetEntity<ScaleHomeFav>()
                .Include(x => x.Store)
                .Include(x => x.Department)
                .Include(x => x.ScaleProducts)
                .Where(x => x.Id == id).FirstOrDefaultAsync();
            return entity;
        }

        /// <summary>
        ///     Insert a entity
        /// </summary>
        /// <param name="entity">Entity to insert</param>
        /// <returns>Entity</returns>
        public override async Task<ScaleHomeFav> Post(ScaleHomeFav entity)
        {
            // return await _repository.TransactionAsync(async context =>
            // {
                // var elem = new List<Store>();
                if (entity.ScaleProducts != null)
                    for (var i = 0; i < entity.ScaleProducts.Count; i++)
                        _repository.GetEntity<ScaleProduct>().Attach(entity.ScaleProducts[i]);
                return await base.Post(entity);
            // });
        }

        /// <summary>
        ///     Update entity
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="entity">Entity to update</param>
        /// <returns>Boolean success</returns>
        /// <exception cref="BusinessLogicException">If id is less to -1</exception>
        public override async Task<bool> Put(long id, ScaleHomeFav entity)
        {
            if (id < 1)
            {
                _logger.LogError("Id parameter out of bounds.");
                throw new BusinessLogicException("Id parameter out of bounds.");
            }

            // var data = await _repository.TransactionAsync(async context =>
            // {
                // remove all stores first
                var tax = await _repository.GetEntity<ScaleHomeFav>()
                    .Include(x => x.ScaleProducts)
                    .Where(x => x.Id == id)
                    .FirstOrDefaultAsync();
                var longList = entity.ScaleProducts.Select(x => x.Id).ToList();
                var removeList = new List<long>();
                var addList = new List<long>();
                var updateList = new List<long>();
                foreach (var store in tax.ScaleProducts.ToList())
                    // Remove the roles which are not in the list of new roles
                    if (!longList.Contains(store.Id))
                    {
                        tax.ScaleProducts.Remove(store);
                        removeList.Add(store.Id);
                    }

                foreach (var newStoreId in longList)
                    // Add the roles which are not in the list of user's roles
                    if (!tax.ScaleProducts.Any(r => r.Id == newStoreId))
                    {
                        addList.Add(newStoreId);
                        var newRole = new ScaleProduct {Id = newStoreId};
                        _repository.GetEntity<ScaleProduct>().Attach(newRole);
                        tax.ScaleProducts.Add(newRole);
                    }
                    else
                    {
                        updateList.Add(newStoreId);
                    }

                entity.ScaleProducts = tax.ScaleProducts;

                return await base.Put(id, entity);
            // });
            // return data;
        }

        protected override IQueryable<ScaleHomeFav> FilterqueryBuilder(
            ScaleHomeFav filter,
            string searchstring,
            string[] splited,
            DbSet<ScaleHomeFav> query)
        {
            IQueryable<ScaleHomeFav> query1 = null;

            if (!string.IsNullOrEmpty(searchstring))
                query1 = query.Where(c =>
                    c.Department.Name.Contains(searchstring) ||
                    c.Store.Name.Contains(searchstring)
                );
            else
                query1 = query
                    .WhereIf(filter.DepartmentId > 0, c => c.DepartmentId == filter.DepartmentId)
                    .WhereIf(filter.StoreId > 0, c => c.StoreId == filter.StoreId);

            query1 = query1
                .Switch(splited)
                .OrderByCase(e => e[0] == "departmentid" && e[1] == "asc", e => e.Department.Name)
                .OrderByDescendingCase(e => e[0] == "departmentid" && e[1] == "desc", e => e.Department.Name)
                .OrderByCase(e => e[0] == "storeid" && e[1] == "asc", e => e.Store.Name)
                .OrderByDescendingCase(e => e[0] == "storeid" && e[1] == "desc", e => e.Store.Name)
                .OrderByDefault(e => e.Department.Name);

            return query1.Include(x => x.Department).IgnoreAutoIncludes().Include(x => x.Store).IgnoreAutoIncludes();
        }
    }
}