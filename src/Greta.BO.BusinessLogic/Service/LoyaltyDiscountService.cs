using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Greta.BO.Api.Abstractions;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Entities.Lite;
using Greta.BO.Api.Sqlserver.Repository;
using Greta.BO.BusinessLogic.Interfaces;
using Greta.BO.BusinessLogic.Specifications.LoyaltyDiscountSpecs;
using Greta.Sdk.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Service;

public interface ILoyaltyDiscountService : IGenericBaseService<LoyaltyDiscount>
{
    Task<LoyaltyDiscount> HaveSameStore(long storeId, long? id = null);
    Task<List<Store>> GetRemainStores();
    Task<LoyaltyDiscount> GetByStore(long sotreId);
}

public class LoyaltyDiscountService : BaseService<ILoyaltyDiscountRepository, LoyaltyDiscount>,
    ILoyaltyDiscountService
{
    public LoyaltyDiscountService(ILoyaltyDiscountRepository repository,
        ISynchroService synchroService,
        ILogger<LoyaltyDiscountService> logger)
        : base(repository, logger, synchroService, Converter)
    {
    }

    private static string Converter(LoyaltyDiscount from) =>
        SynchroDetailRepository.DefaultConverter(LiteLoyaltyDiscount.Convert(from));

    protected override IQueryable<LoyaltyDiscount> FilterqueryBuilder(
        LoyaltyDiscount filter,
        string searchstring,
        string[] splited,
        DbSet<LoyaltyDiscount> query)
    {
        IQueryable<LoyaltyDiscount> query1 = null;

        if (!string.IsNullOrEmpty(searchstring))
            query1 = query.Where(c => c.Name.Contains(searchstring));
        else
            query1 = query.WhereIf(!string.IsNullOrEmpty(filter.Name), c => c.Name.Contains(filter.Name));

        query1 = query1
            .Switch(splited)
            .OrderByCase(e => e[0] == "name" && e[1] == "asc", e => e.Name)
            .OrderByDescendingCase(e => e[0] == "name" && e[1] == "desc", e => e.Name)
            .OrderByDefault(e => e.Name);

        return query1.Include(x => x.Store);
    }

    /// <summary>
    /// Return the discount with this store but diferent to id if id is diferent to null
    /// </summary>
    /// <param name="storeId"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<LoyaltyDiscount> HaveSameStore(long storeId, long? id = null)
    {
        if (id == null)
        {
            return await _repository.GetEntity<LoyaltyDiscount>()
                .Where(x => x.StoreId == storeId)
                .FirstOrDefaultAsync();
        }
        else
        {
            return await _repository.GetEntity<LoyaltyDiscount>()
                .Where(x => x.StoreId == storeId && x.Id != id.Value)
                .FirstOrDefaultAsync();
        }
    }

    public async Task<List<Store>> GetRemainStores()
    {
        var stores = await _repository.GetEntity<LoyaltyDiscount>()
            .Select(x => x.StoreId).ToListAsync();

        return await _repository.GetEntity<Store>()
            .Where(x => !stores.Any(s => s == x.Id))
            .ToListAsync();
    }

    /// <summary>
    /// Return LoyaltyDiscount by store id
    /// </summary>
    /// <param name="storeId"></param>
    /// <returns></returns>
    public async Task<LoyaltyDiscount> GetByStore(long storeId)
    {
        return await _repository.GetEntity<LoyaltyDiscount>()
            .WithSpecification(new LoyaltyDiscountGetByStoreIdSpec(storeId, null))
            .FirstOrDefaultAsync();
    }
}