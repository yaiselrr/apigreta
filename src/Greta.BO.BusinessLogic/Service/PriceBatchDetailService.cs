using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Greta.BO.Api.Abstractions;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.Api.Entities.Lite;
using Greta.BO.Api.Sqlserver.Repository;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Interfaces;
using Greta.BO.BusinessLogic.Specifications.PriceBatchDetailSpecs;
using Greta.Sdk.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Service;

/// <summary>
/// Service layer for price batch detail entity
/// </summary>
public interface IPriceBatchDetailService : IGenericBaseService<PriceBatchDetail>
{
    /// <summary>
    ///     Get productId by upc
    /// </summary>
    /// <param name="upc">upc</param>
    /// <returns></returns>
    Task<long> GetProductIdByUpc(string upc);

    /// <summary>
    ///     Get price batch detail by productId and headerId
    /// </summary>
    /// <param name="productId">productId</param>
    /// <param name="headerId">headerId</param>
    /// <returns></returns>
    Task<PriceBatchDetail> GetByProductAndHEader(long productId, long headerId);

    /// <summary>
    /// Get full entity used for zpl converter or other operation
    /// </summary>
    /// <param name="headerId">header Id</param>
    /// <returns>Entity with all Product reference</returns>
    Task<List<PriceBatchDetail>> GetFullDetails(long headerId);
}

/// <inheritdoc cref="Greta.BO.BusinessLogic.Service.IPriceBatchDetailService" />
public class PriceBatchDetailService : BaseService<IPriceBatchDetailRepository, PriceBatchDetail>, IPriceBatchDetailService
{
    /// <inheritdoc />
    public PriceBatchDetailService(IPriceBatchDetailRepository repository, ILogger<PriceBatchDetailService> logger,
        ISynchroService synchroService)
        : base(repository, logger, synchroService, Converter)
    {
    }

    private static string Converter(PriceBatchDetail from) => SynchroDetailRepository.DefaultConverter(LitePriceBatchDetail.Convert(from));

    /// <summary>
    ///     Insert a entity
    /// </summary>
    /// <param name="entity">Entity to insert</param>
    /// <returns>Entity</returns>
    public override async Task<PriceBatchDetail> Post(PriceBatchDetail entity)
    {
        if (entity.CategoryId == -1) entity.CategoryId = null;
        if (entity.ProductId == -1) entity.ProductId = null;
        if (entity.FamilyId == -1) entity.FamilyId = null;
        var data = await _repository.TransactionAsync(async _ =>
        {
            var specAux = new PriceBatchDetailAuxGetByEntitySpec(entity);
            var priceBatchDetailAux = await _repository.GetEntity<PriceBatchDetail>().WithSpecification(specAux).FirstOrDefaultAsync();

            if (priceBatchDetailAux != null)
            {
                priceBatchDetailAux.Price = entity.Price;
                await _repository.UpdateAsync(priceBatchDetailAux);
                entity = priceBatchDetailAux;
            }
            else
            {
                var id = await _repository.CreateAsync(entity);
                entity.Id = id;
            }

            var specBatch = new BatchGetByEntitySpec(entity.HeaderId);
            var batch = await _repository.GetEntity<Batch>().WithSpecification(specBatch).FirstOrDefaultAsync();

            await synchroService.AddSynchroToStores(
                batch.Stores.Select(x => x.Id).ToList(),
                LitePriceBatchDetail.Convert(entity),
                SynchroType.CREATE);

            return entity;
        });
        return data;
    }

    /// <inheritdoc cref="IGenericBaseService{T}.ChangeState" />
    public override async Task<bool> ChangeState(long id, bool state)
    {
        if (id < 1)
        {
            _logger.LogError("Id parameter out of bounds");
            throw new BusinessLogicException("Id parameter out of bounds");
        }

        var data = await _repository.TransactionAsync(async _ =>
        {
            var success = await _repository.ChangeStateAsync<PriceBatchDetail>(id, state);
            var spec = new PriceBatchDetailGetByIdSpecs(id);
            var entityUpdate = await _repository.GetEntity<PriceBatchDetail>().WithSpecification(spec).FirstOrDefaultAsync();

            await synchroService.AddSynchroToStores(entityUpdate.Header.Stores.Select(x => x.Id).ToList(), LitePriceBatchDetail.Convert(entityUpdate), SynchroType.UPDATE);
            return success;
        });
        return data;
    }

    /// <inheritdoc />
    public async Task<List<PriceBatchDetail>> GetFullDetails(long headerId)
    {

        return await _repository.GetEntity<PriceBatchDetail>()
            .Include("Product.Category")
            .Include("Product.Department")
            .Include("Product.DefaulShelfTag")
            .Include("Family.Products")
            .Include("Family.Products.Category")
            .Include("Family.Products.Department")
            .Include("Family.Products.DefaulShelfTag")
            .Where(x => x.HeaderId == headerId)
            .ToListAsync();
    }

    /// <summary>
    ///     Delete a entity
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>Boolean success</returns>
    /// <exception cref="BusinessLogicException">If id is less to -1</exception>
    public override async Task<bool> Delete(long id)
    {
        if (id < 1)
        {
            _logger.LogError("Id parameter out of bounds");
            throw new BusinessLogicException("Id parameter out of bounds");
        }

        var data = await _repository.TransactionAsync(async _ =>
        {
            var spec = new PriceBatchDetailGetByIdWithoutIncludeSpecs(id);
            var entitieDelete = await _repository.GetEntity<PriceBatchDetail>().WithSpecification(spec).FirstOrDefaultAsync();

            var specBatch = new BatchGetByEntitySpec(entitieDelete.HeaderId);
            var batch = await _repository.GetEntity<Batch>().WithSpecification(specBatch).FirstOrDefaultAsync();

            await synchroService
                .AddSynchroToStores(
                    batch.Stores.Select(x => x.Id).ToList(),
                    entitieDelete,
                    SynchroType.DELETE
                );

            var success = await _repository.DeleteAsync(id);
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
            _logger.LogError("List of ids is null or empty");
            throw new BusinessLogicException("List of ids is null or empty");
        }

        var data = await _repository.TransactionAsync(async _ =>
        {
            var spec = new PriceBatchDetailGetByListIdsWithoutIncludeSpecs(ids);
            var entitiesDelete = await _repository.GetEntity<PriceBatchDetail>().WithSpecification(spec).ToListAsync();

            foreach (var t in entitiesDelete)
            {
                var specBatch = new BatchGetByEntitySpec(t.HeaderId);
                var batch = await _repository.GetEntity<Batch>().WithSpecification(specBatch).FirstOrDefaultAsync();

                await synchroService
                    .AddSynchroToStores(
                        batch.Stores.Select(x => x.Id).ToList(),
                        t,
                        SynchroType.DELETE
                    );
            }

            var success = await _repository.DeleteRangeAsync(entitiesDelete);
            return success;
        });
        return data;
    }

    /// <inheritdoc />
    public async Task<long> GetProductIdByUpc(string upc)
    {
        var spec = new GetProductIdByUpcSpecs(upc);

        return await _repository.GetEntity<Product>().WithSpecification(spec).Select(x => x.Id).FirstOrDefaultAsync();
    }

    /// <inheritdoc />
    public async Task<PriceBatchDetail> GetByProductAndHEader(long productId, long headerId)
    {
        var spec = new PriceBatchDetailGetByProductAndHeaderSpec(productId, headerId);

        return await _repository.GetEntity<PriceBatchDetail>().WithSpecification(spec).FirstOrDefaultAsync();
    }
}