using System.Linq;
using System.Threading.Tasks;
using Greta.BO.Api.Abstractions;
using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Service
{
    public interface IVendorProductService : IGenericBaseService<VendorProduct>
    {
        Task<VendorProduct> GetAllByVendorAndProduct(long? vendorId, long productId);
    }

    public class VendorProductService : BaseService<IVendorProductRepository, VendorProduct>, IVendorProductService
    {
        private IShelfTagService _shelfTagService;

        public VendorProductService(IVendorProductRepository vendorRepository, ILogger<VendorProductService> logger,
            IShelfTagService shelfTagService)
            : base(vendorRepository, logger)
        {
            this._shelfTagService = shelfTagService;
        }

        protected override IQueryable<VendorProduct> FilterqueryBuilder(
            VendorProduct filter,
            string searchstring,
            string[] splited,
            DbSet<VendorProduct> query)
        {
            IQueryable<VendorProduct> query1 = null;

            if (!string.IsNullOrEmpty(searchstring))
                query1 = query.Where(c => c.Vendor.Name.Contains(searchstring)
                                          || c.ProductCode.Contains(searchstring));
            else
                query1 = query.WhereIf(!string.IsNullOrEmpty(filter.ProductCode),
                    c => c.ProductCode.Contains(filter.ProductCode));
            if (filter.VendorId > 0) query1 = query1.Where(x => x.VendorId == filter.VendorId);
            if (filter.ProductId > 0) query1 = query1.Where(x => x.ProductId == filter.ProductId);

            query1 = query1
                .Switch(splited)
                .OrderByCase(e => e[0] == "vendor" && e[1] == "asc", e => e.Vendor.Name)
                .OrderByDescendingCase(e => e[0] == "vendor" && e[1] == "desc", e => e.Vendor.Name)
                .OrderByCase(e => e[0] == "productcode" && e[1] == "asc", e => e.ProductCode)
                .OrderByDescendingCase(e => e[0] == "productcode" && e[1] == "desc", e => e.ProductCode)
                .OrderByDefault(e => e.ProductCode);

            return query1.Include(x => x.Vendor).IgnoreAutoIncludes();
        }

        public async Task<VendorProduct> GetAllByVendorAndProduct(long? vendorId, long productId)
        {
            if (vendorId == null)
            {
                var entities = await _repository.GetEntity<VendorProduct>()
                    .Where(x => x.ProductId == productId)
                    .FirstOrDefaultAsync();
                return entities;
            }
            else
            {
                var entities = await _repository.GetEntity<VendorProduct>()
                    .Where(x => x.VendorId == vendorId.Value && x.ProductId == productId)
                    .FirstOrDefaultAsync();
                return entities;
            }
        }

        /// <summary>
        ///     Insert a entity
        /// </summary>
        /// <param name="entity">Entity to insert</param>
        /// <returns>Entity</returns>
        public override async Task<VendorProduct> Post(VendorProduct entity)
        {
            var data = await _repository.TransactionAsync(async context =>
            {
                var id = await _repository.CreateAsync(entity);
                // entity.Id = id;

                var sPs = await _repository.GetEntity<StoreProduct>()
                    .Where(x => x.ProductId == entity.ProductId)
                    .ToListAsync();

                // var toSyn =
                //             elem.Product.ProductType == ProductType.SPT ?
                //             LiteProduct.Convert(elem.Product, elem) :
                //             elem.Product.ProductType == ProductType.SLP ?
                //             LiteScaleProduct.Convert1((ScaleProduct)elem.Product, elem) :
                //             LiteWProduct.Convert2((WProduct)elem.Product, elem);

                // await synchroService.AddSynchroToStore(
                //    entity.StoreId,
                //    toSyn,
                //    SynchroType.CREATE
                //    );
                return entity;
            });

            var sPs = await _repository.GetEntity<StoreProduct>()
                .Where(x => x.ProductId == entity.ProductId)
                .ToListAsync();
            foreach (var sP in sPs)
            {
                await _shelfTagService.PostFromStoreProduct(sP.Id);
            }

            return data;
        }

        public override async Task<bool> Put(long id, VendorProduct entity)
        {
            if (id < 1)
            {
                _logger.LogError("Id parameter out of bounds.");
                throw new BusinessLogicException("Id parameter out of bounds.");
            }

            var vP = await _repository.GetEntity<VendorProduct>()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            bool needAddShelfTag = (entity.ProductCode != vP.ProductCode) || (entity.CasePack != vP.CasePack);

            var res = await _repository.UpdateAsync(id, entity);
            if (res && needAddShelfTag)
            {
                var sPs = await _repository.GetEntity<StoreProduct>()
                    .Where(x => x.ProductId == vP.ProductId)
                    .ToListAsync();
                foreach (var sP in sPs)
                {
                    await _shelfTagService.PostFromStoreProduct(sP.Id);
                }

                // var toSyn =
                //             elem.Product.ProductType == ProductType.SPT ?
                //             LiteProduct.Convert(elem.Product, elem) :
                //             elem.Product.ProductType == ProductType.SLP ?
                //                 LiteScaleProduct.Convert1((ScaleProduct)elem.Product, elem) :
                //                 LiteWProduct.Convert2((WProduct)elem.Product, elem);
                //
                // await synchroService.AddSynchroToStore(
                //    entity.StoreId,
                //    toSyn,
                //    SynchroType.UPDATE
                //    );
            }

            return res;
        }
    }
}