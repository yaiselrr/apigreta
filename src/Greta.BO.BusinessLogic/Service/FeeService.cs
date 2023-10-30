using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Greta.BO.Api.Abstractions;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.Api.Entities.Lite;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Interfaces;
using Greta.BO.BusinessLogic.Specifications.FeeSpecs;
using Greta.BO.BusinessLogic.Specifications.Generics;
using Greta.Sdk.EFCore.Extensions;
using Greta.Sdk.EFCore.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Service
{
    ///<inheritdoc/>
    public interface IFeeService : IGenericBaseService<Fee>
    {       
    }

    /// <inheritdoc cref="FeeService" />
    public class FeeService : BaseService<IFeeRepository, Fee>, IFeeService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="logger"></param>
        public FeeService(IFeeRepository repository, ILogger<FeeService> logger)
           : base(repository, logger)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="synchroService"></param>
        /// <param name="logger"></param>
        public FeeService(IFeeRepository repository,ISynchroService synchroService, ILogger<FeeService> logger)
            : base(repository, logger, synchroService, Converter)
        {
        }

        private static object Converter(Fee from) => (LiteFee.Convert(from));

        /// <summary>
        ///     Get entity by Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Customer</returns>
        /// <exception cref="BusinessLogicException">If id is less to -1</exception>
        public override async Task<Fee> Get(long id)
        {
            if (id < 1)
            {
                _logger.LogError("Id parameter out of bounds");
                throw new BusinessLogicException("Id parameter out of bounds");
            }

            var entity = await _repository.GetEntity<Fee>().WithSpecification(new FeeGetByIdSpec(id))
                .IgnoreAutoIncludes()
                .FirstOrDefaultAsync();

            return entity;
        }               

        /// <summary>
        ///     Insert a entity
        /// </summary>
        /// <param name="entity">Entity to insert</param>
        /// <returns>Entity</returns>
        public override async Task<Fee> Post(Fee entity)
        {            
            if (entity.Products != null)
                foreach (var t in entity.Products)
                    _repository.GetEntity<Product>().Attach(t);

            if (entity.Families != null)
                foreach (var t in entity.Families)
                    _repository.GetEntity<Family>().Attach(t);

            if (entity.Categories != null)
                foreach (var t in entity.Categories)
                    _repository.GetEntity<Category>().Attach(t);

            return await base.Post(entity);           
        }

        /// <summary>
        ///     Update entity
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="entity">Entity to update</param>
        /// <returns>Boolean success</returns>
        /// <exception cref="BusinessLogicException">If id is less to -1</exception>
        public override async Task<bool> Put(long id, Fee entity)
        {
            if (id < 1)
            {
                _logger.LogError("Id parameter out of bounds");
                throw new BusinessLogicException("Id parameter out of bounds");
            }
            
            var fee = await _repository.GetEntity<Fee>().WithSpecification(new FeeGetByIdSpec(id))               
                .FirstOrDefaultAsync();

            entity.Products = ProcessMany2ManyUpdate(fee.Products, entity.Products);
            entity.Families = ProcessMany2ManyUpdate(fee.Families, entity.Families);
            entity.Categories = ProcessMany2ManyUpdate(fee.Categories, entity.Categories);

            return await base.Put(id, entity);            
        }

        /// <summary>
        /// Command for change state of Fee when there are synchro entities
        /// </summary>
        /// <param name="id"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        /// <exception cref="BusinessLogicException"></exception>
        public override async Task<bool> ChangeState(long id, bool state)
        {
            if (id < 1)
            {
                _logger.LogError("Id parameter out of bounds");
                throw new BusinessLogicException("Id parameter out of bounds.");
            }

            var entity = await _repository.GetEntity<Fee>().Where(x => x.Id == id).FirstOrDefaultAsync();
           
            if (synchroService != null && entity is IFullSyncronizable)
            {
                var data = await _repository.TransactionAsync(async _ =>
                {
                    var success = await _repository.ChangeStateAsync<Fee>(id, state);

                    entity = await _repository.GetEntity<Fee>().WithSpecification(new FeeGetByIdSpec(id)).FirstOrDefaultAsync();

                    var hasMultiplesStores = entity.GetType().GetProperty("Stores");
                    if (hasMultiplesStores != null)
                    {
                        //if have multiples stores only storage on this stores
                        var stores = (List<Store>)hasMultiplesStores.GetValue(entity);
                        if (stores != null)
                            await synchroService.AddSynchroToStores(stores.Select(x => x.Id).ToList(), entity,
                                SynchroType.UPDATE, _converter);
                    }
                    else
                    {
                        var hasOnlyOneStore = entity.GetType().GetProperty("StoreId");
                        if (hasOnlyOneStore != null)
                        {
                            var storeId = (long)hasOnlyOneStore.GetValue(entity)!;
                            await synchroService.AddSynchroToStore(storeId, entity, SynchroType.UPDATE, _converter);
                        }
                        else
                        {
                            await synchroService.AddSynchroToAllStores(entity, SynchroType.UPDATE, _converter);
                        }
                    }

                    return success;
                });
                return data;
            }
            else
            {
                var success = await _repository.ChangeStateAsync<Fee>(id, state);
                return success;
            }
        }
    }
}