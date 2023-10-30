using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Greta.BO.Api.Abstractions;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.Api.Entities.Lite;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Service
{
    public interface IPriceBatchService : IGenericBaseService<PriceBatch>
    {
        Task<PriceBatch> GetByName(string name, long Id = -1);
    }

    public class PriceBatchService : BaseService<IPriceBatchRepository, PriceBatch>, IPriceBatchService
    {
        public PriceBatchService(IPriceBatchRepository repository, ISynchroService synchroService,
            ILogger<PriceBatchService> logger)
            : base(repository, logger, synchroService, Converter)
        {
        }

        private static object Converter(PriceBatch from) => (LitePriceBatch.Convert(from));

        /// <summary>
        ///     Get entity by name
        /// </summary>
        /// <param name="name">PriceBatch name</param>
        /// <returns>PriceBatch</returns>
        public async Task<PriceBatch> GetByName(string name, long Id = -1)
        {
            if (Id == -1)
                return await _repository.GetEntity<PriceBatch>().Where(x => x.Name == name).FirstOrDefaultAsync();
            return await _repository.GetEntity<PriceBatch>().Where(x => x.Name == name && x.Id != Id)
                .FirstOrDefaultAsync();
        }
        
        /// <summary>
        ///     Insert a entity
        /// </summary>
        /// <param name="entity">Entity to insert</param>
        /// <returns>Entity</returns>
        public override async Task<PriceBatch> Post(PriceBatch entity)
        {
            var data = await _repository.TransactionAsync(async context =>
            {
                if (entity.Stores != null)
                    for (var i = 0; i < entity.Stores.Count; i++)
                        _repository.GetEntity<Store>().Attach(entity.Stores[i]);
                
                var id = await _repository.CreateAsync(entity);
            
                await synchroService.AddSynchroToStores(
                    (entity.Stores ?? throw new InvalidOperationException()).Select(x=> x.Id).ToList(),
                    LitePriceBatch.Convert(entity),
                    SynchroType.CREATE);
            
                entity.Id = id;
                return entity;
            });
            return data;
        }

        /// <summary>
        ///     Update entity
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="entity">Entity to update</param>
        /// <returns>Boolean success</returns>
        /// <exception cref="BusinessLogicException">If id is less to -1</exception>
        public override async Task<bool> Put(long id, PriceBatch entity)
        {

            if (id < 1)
            {
                _logger.LogError("Id parameter out of bounds.");
                throw new BusinessLogicException("Id parameter out of bounds.");
            }

            var data = await _repository.TransactionAsync(async context =>
            {
                var priceBatch = await _repository.GetEntity<PriceBatch>()
                    .Include(x => x.Stores)
                    .Where(x => x.Id == id)
                    .FirstOrDefaultAsync();

                //entity.Stores = ProcessMany2ManyUpdate(priceBatch.Stores, entity.Stores);
                
                priceBatch.Name = entity.Name;
                priceBatch.StartTime = entity.StartTime;
                priceBatch.Type = entity.Type;
                
                var success = await base.Put(id, priceBatch);
            
                await synchroService.AddSynchroToStores(
                    entity.Stores.Select(x => x.Id).ToList(),
                    LitePriceBatch.Convert(entity),
                    SynchroType.UPDATE);
            
                return success;
            });
            return data;
        }


        /// <summary>
        ///     Delete a list
        /// </summary>
        /// <param name="ids">List of ids</param>
        /// <returns>Boolean success</returns>
        /// <exception cref="BusinessLogicException">If list is null or empty</exception>
        public override async Task<bool> DeleteRange(List<long> ids)
        {
            if (ids == null || ids.Count == 0)
            {
                _logger.LogError("List of ids is null or empty.");
                throw new BusinessLogicException("List of ids is null or empty.");
            }

            var data = await _repository.TransactionAsync(async context =>
            {
                var entitiesDelete = await _repository.GetEntity<PriceBatch>()
                    .Include(x => x.Stores)
                    .Where(x => ids.Contains(x.Id))
                    .ToListAsync();
            
                foreach (var t in entitiesDelete)
                    await synchroService
                        .AddSynchroToStores(
                            t.Stores.Select(x=>x.Id).ToList(),
                            t,
                            SynchroType.DELETE);
            
                var success = await _repository.DeleteRangeAsync(entitiesDelete);
                return success;
            });
            return data;
        }

        protected override IQueryable<PriceBatch> FilterqueryBuilder(
            PriceBatch filter,
            string searchstring,
            string[] splited,
            DbSet<PriceBatch> query)
        {
            IQueryable<PriceBatch> query1 = null;

            if (!string.IsNullOrEmpty(searchstring))
                query1 = query.Where(c => c.Name.Contains(searchstring));
            else
                query1 = query.WhereIf(!string.IsNullOrEmpty(filter.Name), c => c.Name.Contains(filter.Name));
            //.WhereIf(filter.StartTime != null, c => c.StartTime >= filter.StartTime);

            query1 = query1
                .Switch(splited)
                .OrderByCase(e => e[0] == "name" && e[1] == "asc", e => e.Name)
                .OrderByDescendingCase(e => e[0] == "name" && e[1] == "desc", e => e.Name)
                .OrderByDefault(e => e.Name);

            //return query1.Include(x => x.Stores).IgnoreAutoIncludes
            return query1.Include(x => x.Stores);
            ;
        }
    }
}