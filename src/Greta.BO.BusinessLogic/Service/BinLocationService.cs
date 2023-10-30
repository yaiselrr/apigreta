using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Greta.BO.Api.Abstractions;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Sqlserver;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Interfaces;
using Greta.BO.BusinessLogic.Models.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Service
{
    public interface IBinLocationService : IGenericBaseService<BinLocation>
    {
        Task<BinLocation> GetByName(string name, long Id = -1);
        Task<bool> CanDeleted(long id);
        Task<bool> CanDeleted(List<long> ids);
        Task<List<BinLocation>> GetByStore(long requestStoreId);
        Task<string> AddProductsToBinLocation(BinLocationUPCModel model);
        Task<List<StoreProduct>> GetProductsByBinLocation(long requestBinLocationId);
        Task<bool> DeleteProduct(long requestId);
    }

    public class BinLocationService : BaseService<IBinLocationRepository, BinLocation>, IBinLocationService
    {
        private readonly IShelfTagService _shelfTagService;

        public BinLocationService(IBinLocationRepository binLocationRepository,
            ILogger<BinLocationService> logger,
            IShelfTagService shelfTagService)
            : base(binLocationRepository, logger)
        {
            this._shelfTagService = shelfTagService;
        }

        /// <summary>
        ///     Get entity by name
        /// </summary>
        /// <param name="name">BinLocation name</param>
        /// <returns>BinLocation</returns>
        public async Task<BinLocation> GetByName(string name, long Id = -1)
        {
            if (Id == -1)
                return await _repository.GetEntity<BinLocation>().Where(x => x.Name == name).FirstOrDefaultAsync();
            return await _repository.GetEntity<BinLocation>().Where(x => x.Name == name && x.Id != Id)
                .FirstOrDefaultAsync();
        }

        protected override IQueryable<BinLocation> FilterqueryBuilder(
            BinLocation filter,
            string searchstring,
            string[] splited,
            DbSet<BinLocation> query)
        {
            IQueryable<BinLocation> query1 = null;

            if (!string.IsNullOrEmpty(searchstring))
                query1 = query.Where(c => c.Name.Contains(searchstring));
            else
                query1 = query.WhereIf(!string.IsNullOrEmpty(filter.Name), c => c.Name.Contains(filter.Name))
                    .WhereIf(filter.Store > 0, c => c.Store == filter.Store);

            query1 = query1
                .Switch(splited)
                .OrderByCase(e => e[0] == "name" && e[1] == "asc", e => e.Name)
                .OrderByDescendingCase(e => e[0] == "name" && e[1] == "desc", e => e.Name)
                .OrderByDefault(e => e.Name);

            return query1;
        }

        public async Task<bool> CanDeleted(long id)
        {
            return await _repository.GetEntity<BinLocation>()
                //.Include(e => e.Products)
                //.AnyAsync(e => e.Id == id && e.Products.Any());
                .AnyAsync(e => e.Id == id);
        }

        public async Task<bool> CanDeleted(List<long> ids)
        {
            foreach (var id in ids)
                if (!await CanDeleted(id))
                    return false;
            return true;
        }

        public async Task<List<BinLocation>> GetByStore(long requestStoreId)
        {
            if (requestStoreId < 1)
            {
                _logger.LogError("requestStoreId parameter out of bounds.");
                throw new BusinessLogicException("requestStoreId parameter out of bounds.");
            }

            var entity = await _repository.GetEntity<BinLocation>()
                .Where(x => x.Store == requestStoreId)
                .ToListAsync();
            return entity;
        }

        public async Task<string> AddProductsToBinLocation(BinLocationUPCModel model)
        {
            if (!model.IgnoreCurrents)
            {
                var tor = await _repository.GetEntity<StoreProduct>()
                    .Include(x => x.Product)
                    .Where(x => x.BinLocationId == model.BinLocationId && !model.UPCs.Contains(x.Product.UPC))
                    .ToListAsync();

                foreach (var item in tor)
                {
                    item.BinLocationId = null;
                }

                _repository.GetEntity<StoreProduct>()
                    .UpdateRange(tor);
            }

            var sps = await _repository.GetEntity<StoreProduct>()
                .Include(x => x.Product)
                .Where(x => x.StoreId == model.StoreId && model.UPCs.Contains(x.Product.UPC))
                .ToListAsync();

            foreach (var item in sps)
            {
                item.BinLocationId = model.BinLocationId;
            }

            _repository.GetEntity<StoreProduct>()
                .UpdateRange(sps);
            _repository.GetContext<SqlServerContext>().SaveChanges();

            foreach (var item in sps)
            {
                await _shelfTagService.PostFromStoreProduct(item.Id);
            }

            if (sps.Count == model.UPCs.Count)
            {
                return null;
            }
            else
            {
                return "Some products could not be found in the selected store.";
            }
        }

        public async Task<List<StoreProduct>> GetProductsByBinLocation(long requestBinLocationId)
        {
            if (requestBinLocationId < 1)
            {
                _logger.LogError("requestBinLocationId parameter out of bounds.");
                throw new BusinessLogicException("requestBinLocationId parameter out of bounds.");
            }

            var entity = await _repository.GetEntity<StoreProduct>()
                .Include(x => x.Product)
                .Where(x => x.BinLocation.Id == requestBinLocationId)
                .Select(x => new StoreProduct()
                {
                    Id = x.Id,
                    Product = x.Product == null
                        ? null
                        : new Product()
                        {
                            Id = x.Product.Id,
                            UPC = x.Product.UPC,
                            Name = x.Product.Name
                        },
                })
                .ToListAsync();

            return entity;
        }

        public async Task<bool> DeleteProduct(long requestId)
        {
            var elem = await _repository.GetEntity<StoreProduct>()
                .Where(x => x.Id == requestId)
                .FirstOrDefaultAsync();
            elem.BinLocationId = null;
            _repository.GetEntity<StoreProduct>().Update(elem);
            await _repository.GetContext<SqlServerContext>().SaveChangesAsync();
            return true;
        }

        public override async Task<BinLocation> Post(BinLocation entity)
        {
            UpdateNameBinLocation(entity);
            return await base.Post(entity);
        }

        public override async Task<bool> Put(long id, BinLocation entity)
        {
            UpdateNameBinLocation(entity);
            return await base.Put(id, entity);
        }

        private void UpdateNameBinLocation(BinLocation entitydto)
        {
            entitydto.Name = String.Format("A{0}-{1}-S{2}-S{3}", entitydto.Aisle, (entitydto.Side == 0) ? "L" : "R",
                entitydto.Section, entitydto.Shelf);
        }
    }
}