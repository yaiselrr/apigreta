using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Greta.BO.Api.Abstractions;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Service
{
    public interface IEmployeeService : IGenericBaseService<Employee>
    {
    }


    public class EmployeeService : BaseService<IEmployeeRepository, Employee>, IEmployeeService
    {
        public EmployeeService(IEmployeeRepository employeeRepository, ILogger<EmployeeService> logger, ISynchroService synchroService)
            : base(employeeRepository, logger, synchroService)
        {
        }

        protected override IQueryable<Employee> FilterqueryBuilder(
            Employee filter,
            string searchstring,
            string[] splited,
            DbSet<Employee> query)
        {
            IQueryable<Employee> query1 = null;

            if (!string.IsNullOrEmpty(searchstring))
                query1 = query.Where(c => c.LastName.Contains(searchstring) ||
                                          c.Email.Contains(searchstring) ||
                                          c.FirstName.Contains(searchstring) ||
                                          c.Phone.Contains(searchstring));
            else
                query1 = query.WhereIf(!string.IsNullOrEmpty(filter.LastName),
                        c => c.LastName.Contains(filter.LastName))
                    .WhereIf(!string.IsNullOrEmpty(filter.Email), c => c.Email.Contains(filter.Email))
                    .WhereIf(!string.IsNullOrEmpty(filter.FirstName), c => c.FirstName.Contains(filter.FirstName))
                    .WhereIf(!string.IsNullOrEmpty(filter.Phone), c => c.Phone.Contains(filter.Phone));

            query1 = query1
                .Switch(splited)
                .OrderByCase(e => e[0] == "firstname" && e[1] == "asc", e => e.FirstName)
                .OrderByDescendingCase(e => e[0] == "firstname" && e[1] == "desc", e => e.FirstName)
                .OrderByCase(e => e[0] == "email" && e[1] == "asc", e => e.Email)
                .OrderByDescendingCase(e => e[0] == "email" && e[1] == "desc", e => e.Email)
                .OrderByCase(e => e[0] == "lastname" && e[1] == "asc", e => e.LastName)
                .OrderByDescendingCase(e => e[0] == "lastname" && e[1] == "desc", e => e.LastName)
                .OrderByCase(e => e[0] == "phone" && e[1] == "asc", e => e.Phone)
                .OrderByDescendingCase(e => e[0] == "phone" && e[1] == "desc", e => e.Phone)
                .OrderByDefault(e => e.FirstName);

            return query1;
        }

        /// <summary>
        ///     Insert a entity
        /// </summary>
        /// <param name="entity">Entity to insert</param>
        /// <returns>Entity</returns>
        public override async Task<Employee> Post(Employee entity)
        {
            //get the attached elements
            //tax
            return await _repository.TransactionAsync(async context =>
            {
                // var elem = new List<Store>();
                if (entity.Stores != null)
                    for (var i = 0; i < entity.Stores.Count; i++)
                        _repository.GetEntity<Store>().Attach(entity.Stores[i]);
                // entity.Stores = elem;
                var result = await base.Post(entity);
                var elem = result.Stores.Select(x => x.Id).ToList();
                // result.Stores.Clear();

                await synchroService.AddSynchroToStores(elem, result, SynchroType.CREATE);

                return result;
            });
        }

        /// <summary>
        ///     Update entity
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="entity">Entity to update</param>
        /// <returns>Boolean success</returns>
        /// <exception cref="BusinessLogicException">If id is less to -1</exception>
        public override async Task<bool> Put(long id, Employee entity)
        {
            if (id < 1)
            {
                _logger.LogError("Id parameter out of bounds.");
                throw new BusinessLogicException("Id parameter out of bounds.");
            }

            var data = await _repository.TransactionAsync(async context =>
            {
                // remove all stores first
                var tax = await _repository.GetEntity<Tax>()
                    .Include(x => x.Stores)
                    .Where(x => x.Id == id)
                    .FirstOrDefaultAsync();

                //entity.Stores = ProcessMany2ManyUpdate<Store>(tax.Stores, entity.Stores);

                var longList = entity.Stores.Select(x => x.Id).ToList();
                var removeList = new List<long>();
                var addList = new List<long>();
                var updateList = new List<long>();
                foreach (var store in tax.Stores.ToList())
                    // Remove the roles which are not in the list of new roles
                    if (!longList.Contains(store.Id))
                    {
                        tax.Stores.Remove(store);
                        removeList.Add(store.Id);
                    }

                foreach (var newStoreId in longList)
                    // Add the roles which are not in the list of user's roles
                    if (!tax.Stores.Any(r => r.Id == newStoreId))
                    {
                        addList.Add(newStoreId);
                        var newRole = new Store { Id = newStoreId };
                        _repository.GetEntity<Store>().Attach(newRole);
                        tax.Stores.Add(newRole);
                    }
                    else
                    {
                        updateList.Add(newStoreId);
                    }

                entity.Stores.Clear();

                await synchroService.AddSynchroToStores(addList, entity, SynchroType.CREATE);
                await synchroService.AddSynchroToStores(updateList, entity, SynchroType.UPDATE);
                await synchroService.AddSynchroToStores(removeList, entity, SynchroType.DELETE);

                entity.Stores = tax.Stores;

                return await base.Put(id, entity);
            });
            return data;
        }
    }
}