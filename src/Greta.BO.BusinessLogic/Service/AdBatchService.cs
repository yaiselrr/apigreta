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
    public interface IAdBatchService : IGenericBaseService<AdBatch>
    {
        Task<AdBatch> GetByName(string name, long Id = -1);
    }

    public class AdBatchService : BaseService<IAdBatchRepository, AdBatch>, IAdBatchService
    {
        public AdBatchService(IAdBatchRepository repository, ISynchroService synchroService,
            ILogger<AdBatchService> logger)
            : base(repository, logger, synchroService, Converter)
        {
        }

        private static object Converter(AdBatch from) => (LiteAdBatch.Convert(from));

        /// <summary>
        ///     Get entity by name
        /// </summary>
        /// <param name="name">AdBatch name</param>
        /// <returns>AdBatch</returns>
        public async Task<AdBatch> GetByName(string name, long Id = -1)
        {
            if (Id == -1)
                return await _repository.GetEntity<AdBatch>().Where(x => x.Name == name).FirstOrDefaultAsync();
            return await _repository.GetEntity<AdBatch>().Where(x => x.Name == name && x.Id != Id)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        ///     Insert a entity
        /// </summary>
        /// <param name="entity">Entity to insert</param>
        /// <returns>Entity</returns>
        public override async Task<AdBatch> Post(AdBatch entity)
        {
            var data = await _repository.TransactionAsync(async context =>
            {
                if (entity.Stores != null)
                    for (var i = 0; i < entity.Stores.Count; i++)
                        _repository.GetEntity<Store>().Attach(entity.Stores[i]);

                var id = await _repository.CreateAsync(entity);

                await synchroService.AddSynchroToStores(
                    (entity.Stores ?? throw new InvalidOperationException()).Select(x => x.Id).ToList(),
                    LiteAdBatch.Convert(entity),
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
        public override async Task<bool> Put(long id, AdBatch entity)
        {
            if (id < 1)
            {
                _logger.LogError("Id parameter out of bounds.");
                throw new BusinessLogicException("Id parameter out of bounds.");
            }

            var data = await _repository.TransactionAsync(async context =>
            {
                var adBatch = await _repository.GetEntity<AdBatch>()
                    .Include(x => x.Stores)
                    .Where(x => x.Id == id)
                    .FirstOrDefaultAsync();

                //entity.Stores = ProcessMany2ManyUpdate(priceBatch.Stores, entity.Stores);

                adBatch.Name = entity.Name;
                adBatch.StartTime = entity.StartTime;
                adBatch.EndTime = entity.StartTime;
                adBatch.Type = entity.Type;

                var success = await base.Put(id, adBatch);

                await synchroService.AddSynchroToStores(
                    entity.Stores.Select(x => x.Id).ToList(),
                    LiteAdBatch.Convert(entity),
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
                var entitiesDelete = await _repository.GetEntity<AdBatch>()
                    .Include(x => x.Stores)
                    .Where(x => ids.Contains(x.Id))
                    .ToListAsync();

                foreach (var t in entitiesDelete)
                    await synchroService
                        .AddSynchroToStores(
                            t.Stores.Select(x => x.Id).ToList(),
                            t,
                            SynchroType.DELETE);

                var success = await _repository.DeleteRangeAsync(entitiesDelete);
                return success;
            });
            return data;
        }

        protected override IQueryable<AdBatch> FilterqueryBuilder(
            AdBatch filter,
            string searchstring,
            string[] splited,
            DbSet<AdBatch> query)
        {
            IQueryable<AdBatch> query1 = null;

            if (!string.IsNullOrEmpty(searchstring))
                query1 = query.Where(c => c.Name.Contains(searchstring));
            else
                query1 = query.WhereIf(!string.IsNullOrEmpty(filter.Name), c => c.Name.Contains(filter.Name));
            // .WhereIf(filter.StartTime != null, c => c.StartTime >= filter.StartTime)
            // .WhereIf(filter.EndTime != null, c => c.EndTime <= filter.EndTime);

            query1 = query1
                .Switch(splited)
                .OrderByCase(e => e[0] == "name" && e[1] == "asc", e => e.Name)
                .OrderByDescendingCase(e => e[0] == "name" && e[1] == "desc", e => e.Name)
                .OrderByDefault(e => e.Name);

            return query1.Include(x => x.Stores).IgnoreAutoIncludes();
        }
    }
}