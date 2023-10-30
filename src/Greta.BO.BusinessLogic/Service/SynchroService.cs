using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Greta.BO.Api.Abstractions;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.Api.Entities.Lite;
using Greta.BO.BusinessLogic.Interfaces;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.Sdk.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Service;

public interface ISynchroService : IGenericBaseService<Synchro>
{
    Task AddSynchroToAllStores<TDat>(TDat data, SynchroType type, Func<TDat, object> converter = null);
    Task AddSynchroToStore<TDat>(long store, TDat data, SynchroType type, Func<TDat, object> converter = null);
    Task<List<Synchro>> GetPathsLeftForStore(long storeId);

    Task AddSynchroToStores<TDat>(List<long> stores, TDat data, SynchroType type,
        Func<TDat, object> converter = null);

    Task<Synchro> GetOpenSynchroById(long id);
    Task<bool> HasSynchroInProgress(long id);
    Task<DownSynchroCompilation> CreateAFullBackupFile(long storeId);
    Task<SynchroStatistics> GetStatisticsByStore(long requestStoreId);
    Task<List<Synchro>> GetPathsLeftForDevice(string deviceId, int synchroVersion, long storeId);
}

public class SynchroService : BaseService<ISynchroRepository, Synchro>, ISynchroService
{
    private readonly IStoreRepository storeRepository;
    private readonly ISynchroDetailRepository synchroDRepository;

    public SynchroService(
        IStoreRepository storeRepository,
        ISynchroRepository synchroRepository,
        ISynchroDetailRepository synchroDRepository,
        ILogger<SynchroService> logger)
        : base(synchroRepository, logger)
    {
        this.storeRepository = storeRepository;
        this.synchroDRepository = synchroDRepository;
    }

    public async Task AddSynchroToAllStores<TDat>(TDat data, SynchroType type, Func<TDat, object> converter = null)
    {
        var stores = await storeRepository.GetEntity<Store>()
            .Where(x => x.Devices.Any())
            .Select(x => x.Id)
            .ToListAsync();
        await AddSynchroToStores(stores, data, type, converter);
    }

    public async Task AddSynchroToStores<TDat>(List<long> stores, TDat data, SynchroType type,
        Func<TDat, object> converter = null)
    {
        foreach (var i in stores) await AddSynchroToStore(i, data, type, converter);
    }

    public async Task AddSynchroToStore<TDat>(long store, TDat data, SynchroType type,
        Func<TDat, object> converter = null)
    {
        var synchro = await _repository.GetOpenSynchroForStore(store);
        if (!await synchroDRepository.CreateSynchroDetail(
                synchro,
                data,
                type,
                converter
            ))
            throw new AggregateException("We can not add this element to the synchronization queue.");
    }

    public async Task<Synchro> GetOpenSynchroById(long id)
    {
        return await _repository.GetEntity<Synchro>()
            .Include(x => x.SynchroDetails)
            .Where(x => x.Id == id)
            .Where(x => x.Status == SynchroStatus.OPEN)
            .FirstOrDefaultAsync();
    }

    /// <summary>
    /// Return if we have one synchro processing
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<bool> HasSynchroInProgress(long id)
    {
        return await _repository.GetEntity<Synchro>()
            .Where(x => x.StoreId == id)
            .Where(x => x.Status == SynchroStatus.CLOSE)
            .FirstOrDefaultAsync() == null;
    }

    /// <summary>
    /// Create a full backup
    /// </summary>
    /// <param name="storeId"></param>
    /// <returns></returns>
    public async Task<DownSynchroCompilation> CreateAFullBackupFile(long storeId)
    {
        var data = new DownSynchroCompilation();

        var store = await _repository.GetEntity<Store>()
            .Include(x => x.Taxs)
            .Where(x => x.Id == storeId)
            .FirstOrDefaultAsync();
        
        data.StoreConfiguration = LiteStore.Convert(store);

        var products = await _repository.GetEntity<Product>()
            // .Include(x => x.MixAndMatchs)
            .LeftJoin(
                _repository.GetEntity<StoreProduct>().Where(x => x.StoreId == storeId).Include(x => x.Taxs),
                product => product.Id,
                storeP => storeP.ProductId,
                (product, storeP) => new
                {
                    Product = product,
                    StoreP = storeP
                }
            )
            .Where(x =>
                x.StoreP != null)
            .ToListAsync();

        var productIds = products.Select(p => p.Product.Id).ToList();

        //All This element need to be synchro not need be filter by store
        data.LoyaltyDiscount = LiteLoyaltyDiscount.Convert(await _repository.GetEntity<LoyaltyDiscount>()
            .Where(x => x.StoreId == storeId)
            .FirstOrDefaultAsync());

        data.ReasonCodes = (await _repository.GetEntity<ScaleReasonCodes>()
                .ToListAsync())
            .Select(LiteScaleReasonCodes.Convert).ToList();

        data.Profiles = (await _repository.GetEntity<Profiles>()
                .Where(x => x.ApplicationId == 2)
                .Include(x => x.Permissions)
                .ToListAsync())
            .Select(LiteProfile.Convert).ToList();

        data.Employees = (await _repository.GetEntity<BOUser>()
                .Where(x => x.POSProfileId != null && x.Stores.Any(s => s.Id == storeId))
                .ToListAsync())
            .Select(LiteEmployee.Convert).ToList();

        data.TenderTypes = (await _repository.GetEntity<TenderType>().ToListAsync())
            .Select(LiteTenderType.Convert).ToList();

        data.Vendors = (await _repository.GetEntity<Vendor>().ToListAsync())
            .Select(LiteVendor.Convert).ToList();

        data.Families = (await _repository.GetEntity<Family>().ToListAsync())
            .Select(LiteFamily.Convert).ToList();

        data.Discounts = (await _repository.GetEntity<Discount>()
                .Include(p => p.Products.Where(x => productIds.Contains(x.Id)))
                .ToListAsync())
            .Select(LiteDiscount.Convert).ToList();
        
        data.Departments = (await _repository.GetEntity<Department>().ToListAsync())
            .Select(LiteDepartment.Convert).ToList();

        data.Categories = (await _repository.GetEntity<Category>().ToListAsync())
            .Select(x => LiteCategory.Convert(x, store.Taxs.Select(t => t.Id).ToList())).ToList();

        data.ScaleCategories = (await _repository.GetEntity<ScaleCategory>().ToListAsync())
            .Select(LiteScaleCategory.Convert).ToList();

        data.ExternalScales =
            (await _repository.GetEntity<ExternalScale>()
                .Include(x => x.Departments)
                .Include(x => x.SyncDevice)
                .Where(x => x.StoreId == storeId).ToListAsync())
            .Select(LiteExternalScale.Convert).ToList();

        data.ScaleHomeFavs =
            (await _repository.GetEntity<ScaleHomeFav>().Where(x => x.StoreId == storeId).ToListAsync())
            .Select(LiteScaleHomeFav.Convert).ToList();

        data.Taxes = (await _repository.GetEntity<Tax>()
                .Where(x => x.Stores.Any(s => s.Id == storeId))
                .ToListAsync())
            .Select(LiteTax.Convert).ToList();        
        // data.Taxes = (store.Taxs)
        //     .Select(LiteTax.Convert).ToList();

        data.ScaleLabelTypes = (await _repository.GetEntity<ScaleLabelType>()
                .ToListAsync())
            .Select(LiteScaleLabelType.Convert).ToList();

        data.ScaleLabelDefinitions = new List<LiteScaleLabelDefinition>();
        var def = (await _repository.GetEntity<ScaleLabelDefinition>().ToListAsync())
            .Select(LiteScaleLabelDefinition.Convert).ToList();
        foreach (var d in def.Where(d => productIds.Contains(d.ScaleProductId)))
        {
            data.ScaleLabelDefinitions.Add(d);
        }

        data.Fees = (await _repository.GetEntity<Fee>()
                .Include(p => p.Products)
                .Include(p => p.Categories)
                .Include(p => p.Families)
                .ToListAsync())
            .Select(LiteFee.Convert).ToList();

        data.MixAndMatchs = (await _repository.GetEntity<MixAndMatch>()
                .Include(p => p.Products)
                .Include(p => p.Families)
                .ToListAsync())
            .Select(LiteMixAndMatch.Convert).ToList();

        data.Products = products.Where(x => x.Product.ProductType == ProductType.SPT)
            .Select(x => LiteProduct.Convert(x.Product, x.StoreP)).ToList();

        data.ScaleProducts = new List<LiteScaleProduct>();
        foreach (var v in products)
        {
            if (v.Product is ScaleProduct sp)
            {
                data.ScaleProducts.Add(LiteScaleProduct.Convert1(sp, v.StoreP));
            }
        }

        // data.ScaleProducts = products.Where(x => x.Product.ProductType == ProductType.SLP)
        //     .Select(x => LiteScaleProduct.Convert1((ScaleProduct)x.Product, x.StoreP)).ToList();

        // data.WProducts = products.Where(x => x.Product.ProductType == ProductType.CWP)
        //     .Select(x => LiteWProduct.Convert2((WProduct)x.Product, x.StoreP)).ToList();

        data.KitProducts = products.Where(x => x.Product.ProductType == ProductType.KPT)
            .Select(x => LiteKitProduct.Convert2((KitProduct)x.Product, x.StoreP)).ToList();

        var pbd = new List<PriceBatchDetail>();
        var pb = await _repository.GetEntity<PriceBatch>()
            .Include(x => x.PriceBatchDetails)
            .Where(b => b.Stores.Any(s => s.Id == storeId))
            .ToListAsync();
        foreach (var p in pb)
        {
            pbd.AddRange(p.PriceBatchDetails);
        }

        data.PriceBatches = pb
            .Select(LitePriceBatch.Convert).ToList();

        var ab = await _repository.GetEntity<AdBatch>()
            .Include(x => x.PriceBatchDetails)
            .Where(b => b.Stores.Any(s => s.Id == storeId))
            .ToListAsync();
        foreach (var p in ab)
        {
            pbd.AddRange(p.PriceBatchDetails);
        }

        data.AdBatches = ab
            .Select(LiteAdBatch.Convert).ToList();

        data.PriceBatchDetails = pbd
            .Select(LitePriceBatchDetail.Convert).ToList();

        data.Customers = (await _repository.GetEntity<Customer>().ToListAsync())
            .Select(LiteCustomer.Convert).ToList();

        return data;
    }

    public async Task<SynchroStatistics> GetStatisticsByStore(long storeId)
    {
        var result = new SynchroStatistics();
        result.Open = await _repository.GetEntity<Synchro>()
            .Where(x => x.StoreId == storeId)
            .Where(x => x.Status == SynchroStatus.OPEN)
            .CountAsync();
        result.Process = await _repository.GetEntity<Synchro>()
            .Where(x => x.StoreId == storeId)
            .Where(x => x.Status == SynchroStatus.CLOSE)
            .CountAsync();
        result.Complete = await _repository.GetEntity<Synchro>()
            .Where(x => x.StoreId == storeId)
            .Where(x => x.Status == SynchroStatus.COMPLETE)
            .CountAsync();
        return result;
    }

    public async Task<List<Synchro>> GetPathsLeftForDevice(string deviceId, int synchroVersion,
        long storeId)
    {
        var synchros = await _repository.GetEntity<Synchro>()
            .Where(s => s.Tag > synchroVersion && s.Status == SynchroStatus.CLOSE && s.StoreId == storeId)
            .OrderBy(x => x.Tag)
            .ToListAsync();

        return synchros;
    }

    public async Task<List<Synchro>> GetPathsLeftForStore(long storeId)
    {
        var synchros = await _repository.GetEntity<Synchro>()
            .Where(s => s.StoreId == storeId && s.Status == SynchroStatus.CLOSE)
            .OrderBy(x => x.Tag)
            .ToListAsync();

        return synchros;
    }
}