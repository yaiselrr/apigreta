using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Greta.BO.Api.Abstractions;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.Api.Entities.Events.Internal.Products;
using Greta.BO.Api.Entities.Lite;
using Greta.BO.Api.Sqlserver;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Interfaces;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Models.Dto.Search;
using Greta.BO.BusinessLogic.Specifications.FamilySpecs;
using Greta.BO.BusinessLogic.Specifications.Generics;
using Greta.BO.BusinessLogic.Specifications.InventorySpecs;
using Greta.Sdk.Core.Models.Pager;
using Greta.Sdk.EFCore.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Service
{
    public interface IStoreProductService : IGenericBaseService<StoreProduct>
    {
        Task<StoreProduct> GetAllByProductAndStore(long productId, long storeId);
        Task<List<StoreProduct>> GetAllByProduct(long productId);
        Task<bool> CreateOnMultipleStores(List<long> stores, StoreProduct entity);
        Task<StoreProduct> PostImport(StoreProduct entity, Product product);

        /// <summary>
        /// Update order amount
        /// </summary>
        Task<bool> PutByOrderAmount(long id, decimal orderAmount);

        /// <summary>
        /// Filter and paginate inventory
        /// </summary>
        Task<Pager<StoreProduct>> FilterInventory(InventorySearchModel filter, int currentPage, int pageSize,
            long storeId);

        /// <summary>
        /// Filter and paginate suggested inventory 
        /// </summary>
        Task<Pager<StoreProduct>> FilterPaginatedSuggested(InventorySearchModel filter, int currentPage, int pageSize,
            long storeId, long vendorId);

        /// <summary>
        /// Filter suggested
        /// </summary>
        Task<List<StoreProduct>> FilterSuggested(InventorySearchModel filter, long storeId, long vendorId);

        /// <summary>
        /// Update inventory from model
        /// </summary>
        Task<StoreProduct> UpdateInventory(InventoryUpdateModel updateModel);

        /// <summary>
        /// Pre process fiscal inventory
        /// </summary>
        Task<InventoryFiscalModel> PreprocessFiscalInventory(InventoryFiscalModel model);

        /// <summary>
        /// Process fiscal inventory
        /// </summary>
        Task<bool> ProcessFiscalInventory(InventoryFiscalModel model);

        Task<Pager<StoreProduct>> FilteBinlocation(
            int currentPage,
            int pageSize,
            string searchString,
            string sortString,
            long binLocationId
        );

        Task<Pager<Product>> FilterByStore(
            int currentPage,
            int pageSize,
            string searchString,
            string sortString,
            string uPC,
            string name,
            long storeId
        );

        Task<Product> GetProductByUPC(long storeId, string upc);
        Task<StoreProduct> GetWithParent(long child);
        Task<StoreProduct> GetStoreProductByUPC(long requestStoreId, string requestUpc);
        Task<decimal> GetCostByProductAndStore(long deviceStoreId, long tProductId);
        Task<StoreProduct> GetWithProduct(long productId);

        void UpdateValuesNormal(StoreProduct entity);

        /// <summary>
        /// Update price and gross profit when changes the target Gross Profit of Category
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="targetGrossProfit"></param>
        void UpdateValuesByTargetGrossProfit(StoreProduct entity, decimal targetGrossProfit);

        /// <summary>
        /// Update Price and GrossProfit, the calculus si different if have CategoryChange
        /// </summary>
        /// <param name="entity"></param>
        void UpdateValues(StoreProduct entity);
    }

    public class StoreProductService : BaseService<IStoreProductRepository, StoreProduct>, IStoreProductService
    {
        private IQtyHandTrackRepository _qtyRepository;
        private IShelfTagService _shelfTagService;
        private IRoundingTableService _roundingTableService;
        private readonly IMediator _mediator;

        public StoreProductService(
            IStoreProductRepository repository,
            IQtyHandTrackRepository qtyRepository,
            IShelfTagService shelfTagService,
            IRoundingTableService roundingTableService,
            ISynchroService synchroService,
            IMediator mediator,
            ILogger<StoreProductService> logger)
            : base(repository, logger, synchroService)
        {
            this._qtyRepository = qtyRepository;
            this._shelfTagService = shelfTagService;
            this._roundingTableService = roundingTableService;
            _mediator = mediator;
        }

        public async Task<InventoryFiscalModel> PreprocessFiscalInventory(InventoryFiscalModel model)
        {
            foreach (var item in model.Items)
            {
                var upcNew = (item.UPC.Length == 13) ? item.UPC.Substring(0, 12) : item.UPC;
                var p = await _repository.GetEntity<StoreProduct>()
                    .Include(e => e.Product)
                    .Where(x =>
                        (x.Product.UPC == upcNew || x.Product.UPC == item.UPC)
                        && x.StoreId == model.StoreId)
                    .FirstOrDefaultAsync();

                var sales = await _repository.GetEntity<SaleProduct>()
                    .Include(x => x.Sale)
                    .Where(x => x.Sale.SaleTime > model.DateTime.ToUniversalTime() && x.ProductId == p.Id)
                    .ToListAsync();
                item.CountSold = sales.Sum(x => x.QTY);
                item.QtyHand = p.QtyHand;
                item.Name = p.Product.Name;
                item.Id = p.Id;
                item.Update = false;
            }

            return model;
        }

        public async Task<bool> ProcessFiscalInventory(InventoryFiscalModel model)
        {
            foreach (var item in model.Items)
            {
                var p = await _repository.GetEntity<StoreProduct>()
                    .WithSpecification(new InventoryGetForProcessFiscalSpec(item.Id, model.StoreId))
                    .FirstOrDefaultAsync();
                if (p != null)
                {
                    if (item.Update)
                    {
                        //QtyHandTask
                        if (item.Count != 0)
                        {
                            await _qtyRepository.CreateQtyHandTrack(p.Product, p.Store, p.QtyHand,
                                p.QtyHand + item.Count);
                        }

                        p.QtyHand += item.Count;
                    }
                    else
                    {
                        //QtyHandTask
                        if (p.QtyHand != item.Count)
                        {
                            await _qtyRepository.CreateQtyHandTrack(p.Product, p.Store, p.QtyHand,
                                item.Count);
                        }

                        p.QtyHand = item.Count;
                    }

                    _repository.GetEntity<StoreProduct>().Update(p);
                    await _repository.GetContext<SqlServerContext>().SaveChangesAsync();
                }
            }

            return true;
        }

        public async Task<Product> GetProductByUPC(long storeId, string upc)
        {
            // return await _repository.GetEntity<StoreProduct>()
            //     .Include(e => e.Product)
            //     .Where(x => (upc.StartsWith(x.Product.UPC) || x.Product.UPC.StartsWith(upc))&& x.StoreId == storeId)
            //     .Select(x => new Product() {
            //         Id = x.Product.Id,
            //         UPC = x.Product.UPC,
            //         Name = x.Product.Name
            //     })
            //     .FirstOrDefaultAsync();

            var upcNew = (upc.Length == 13) ? upc.Substring(0, 12) : upc;
            return await _repository.GetEntity<StoreProduct>()
                .Include(e => e.Product)
                .Where(x => (x.Product.UPC == upcNew || x.Product.UPC == upc) && x.StoreId == storeId)
                .Select(x => new Product()
                {
                    Id = x.Product.Id,
                    UPC = x.Product.UPC,
                    Name = x.Product.Name
                })
                .FirstOrDefaultAsync();
        }

        public async Task<StoreProduct> GetWithParent(long child)
        {
            return await _repository.GetEntity<StoreProduct>()
                .Include(e => e.Product)
                .Include(e => e.Parent)
                .Include(e => e.Child)
                .Where(x => x.Id == child)
                .FirstOrDefaultAsync();
        }

        public async Task<StoreProduct> GetStoreProductByUPC(long storeId, string upc)
        {
            var upcNew = (upc.Length == 13) ? upc.Substring(0, 12) : upc;
            return await _repository.GetEntity<StoreProduct>()
                .Include(e => e.Product)
                .Where(x => (x.Product.UPC == upcNew || x.Product.UPC == upc) && x.StoreId == storeId)
                .Select(x => new StoreProduct()
                {
                    Id = x.Id,
                    Product = x.Product,
                    QtyHand = x.QtyHand,
                    OrderAmount = x.OrderAmount
                })
                .FirstOrDefaultAsync();
        }

        public async Task<decimal> GetCostByProductAndStore(long deviceStoreId, long tProductId)
        {
            return await _repository.GetEntity<StoreProduct>()
                .Where(x => x.StoreId == deviceStoreId && x.ProductId == tProductId).Select(x => x.Cost)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Query for paginate and filter inventory
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="storeId"></param>
        /// <returns></returns>
        /// <exception cref="BusinessLogicException"></exception>
        public async Task<Pager<StoreProduct>> FilterInventory(InventorySearchModel filter, int currentPage,
            int pageSize, long storeId)
        {
            if (currentPage < 1 || pageSize < 1)
            {
                _logger.LogError("Page parameter (currentPage or pageSize) out of bounds");
                throw new BusinessLogicException("Page parameter out of bounds");
            }

            var query = _repository.GetEntity<StoreProduct>()
                .WithSpecification(new InventoryFilterSpec(filter, currentPage, pageSize, storeId));

            var entities = await query.Select(x => new StoreProduct()
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
                BinLocation = x.BinLocation == null
                    ? null
                    : new BinLocation()
                    {
                        Id = x.BinLocation.Id,
                        Name = x.BinLocation.Name
                    },
                QtyHand = x.QtyHand,
                OrderTrigger = x.OrderTrigger,
                OrderAmount = x.OrderAmount
            }).ToPageAsync(currentPage, pageSize);
            return entities;
        }

        /// <summary>
        /// Filter and paginate suggested inventory
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="storeId"></param>
        /// <param name="vendorId"></param>
        /// <returns></returns>
        /// <exception cref="BusinessLogicException"></exception>
        public async Task<Pager<StoreProduct>> FilterPaginatedSuggested(InventorySearchModel filter, int currentPage,
            int pageSize, long storeId, long vendorId)
        {
            if (currentPage < 1 || pageSize < 1)
            {
                _logger.LogError("Page parameter (currentPage or pageSize) out of bounds");
                throw new BusinessLogicException("Page parameter out of bounds");
            }

            var query1 = GetSuggested(filter, pageSize, storeId, vendorId);

            return await query1.Select(x => new StoreProduct()
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
                QtyHand = x.QtyHand,
                OrderTrigger = x.OrderTrigger,
                OrderAmount = x.OrderAmount
            }).ToPageAsync(currentPage, pageSize);
        }

        /// <summary>
        /// Filter uggested
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="storeId"></param>
        /// <param name="vendorId"></param>
        /// <returns></returns>
        public async Task<List<StoreProduct>> FilterSuggested(InventorySearchModel filter, long storeId, long vendorId)
        {
            var query1 = GetSuggested(filter, 0, storeId, vendorId);

            return await query1.Select(x => new StoreProduct()
            {
                Id = x.Id,
                Product = x.Product == null
                    ? null
                    : new Product()
                    {
                        Id = x.Product.Id,
                        UPC = x.Product.UPC,
                        Name = x.Product.Name,
                        VendorProducts = x.Product.VendorProducts.Where(vp => vp.VendorId == vendorId).ToList()
                    },
                ProductId = x.ProductId,
                QtyHand = x.QtyHand,
                OrderTrigger = x.OrderTrigger,
                OrderAmount = x.OrderAmount
            }).ToListAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="pageSize"></param>
        /// <param name="storeId"></param>
        /// <param name="vendorId"></param>
        /// <returns></returns>
        private IQueryable<StoreProduct> GetSuggested(InventorySearchModel filter, long pageSize, long storeId,
            long vendorId)
            => _repository.GetEntity<StoreProduct>()
                .WithSpecification(new InventoryGetSuggestedSpec(filter, storeId, vendorId));

        public async Task<Pager<StoreProduct>> FilteBinlocation(
            int currentPage,
            int pageSize,
            string searchString,
            string sortString,
            long binLocationId
        )
        {
            if (currentPage < 1 || pageSize < 1)
            {
                _logger.LogError("Page parameter (currentPage or pageSize) out of bounds.");
                throw new BusinessLogicException("Page parameter out of bounds.");
            }

            var splited = string.IsNullOrEmpty(sortString) ? new[] { "", "" } : sortString.Split("_");

            IQueryable<StoreProduct> query1 = _repository.GetEntity<StoreProduct>()
                .Where(x => x.BinLocationId == binLocationId);

            if (!string.IsNullOrEmpty(searchString))
                query1 = query1.Where(c =>
                    c.Product.Name.Contains(searchString) ||
                    c.Product.UPC.Contains(searchString) ||
                    c.BinLocation.Name.Contains(searchString)
                );

            query1 = query1
                .Switch(splited)
                .OrderByCase(e => e[0] == "upc" && e[1] == "asc", e => e.Product.UPC)
                .OrderByDescendingCase(e => e[0] == "upc" && e[1] == "desc", e => e.Product.UPC)
                .OrderByCase(e => e[0] == "productName" && e[1] == "asc", e => e.Product.Name)
                .OrderByDescendingCase(e => e[0] == "productName" && e[1] == "desc", e => e.Product.Name)
                .OrderByDefault(e => e.Product.UPC);


            var entities = await query1
                .Include(x => x.Product)
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
                        }
                })
                .ToPageAsync(currentPage, pageSize);
            return entities;
        }

        public async Task<Pager<Product>> FilterByStore(
            int currentPage,
            int pageSize,
            string searchString,
            string sortString,
            string uPC,
            string name,
            long storeId
        )
        {
            if (currentPage < 1 || pageSize < 1)
            {
                _logger.LogError("Page parameter (currentPage or pageSize) out of bounds.");
                throw new BusinessLogicException("Page parameter out of bounds.");
            }

            var splited = string.IsNullOrEmpty(sortString) ? new[] { "", "" } : sortString.Split("_");

            IQueryable<StoreProduct> query1 = _repository.GetEntity<StoreProduct>()
                .Where(x => x.StoreId == storeId);

            if (!string.IsNullOrEmpty(searchString))
                query1 = query1.Where(c =>
                    c.Product.Name.Contains(searchString) ||
                    c.Product.UPC.Contains(searchString)
                );

            query1 = query1
                .WhereIf(!string.IsNullOrEmpty(uPC), c => c.Product.UPC.Contains(uPC))
                .WhereIf(!string.IsNullOrEmpty(name), c => c.Product.Name.Contains(name));

            query1 = query1
                .Switch(splited)
                .OrderByCase(e => e[0] == "upc" && e[1] == "asc", e => e.Product.UPC)
                .OrderByDescendingCase(e => e[0] == "upc" && e[1] == "desc", e => e.Product.UPC)
                .OrderByCase(e => e[0] == "productName" && e[1] == "asc", e => e.Product.Name)
                .OrderByDescendingCase(e => e[0] == "productName" && e[1] == "desc", e => e.Product.Name)
                .OrderByDefault(e => e.Product.UPC);


            var entities = await query1
                .Include(x => x.Product)
                .Select(x => new Product()
                {
                    Id = x.Product.Id,
                    UPC = x.Product.UPC,
                    Name = x.Product.Name
                })
                .ToPageAsync(currentPage, pageSize);
            return entities;
        }

        public async Task<StoreProduct> UpdateInventory(InventoryUpdateModel updateModel)
        {
            var entity = await _repository.GetEntity<StoreProduct>()
                .WithSpecification(new InventoryGetStoreProductByIdSpec(updateModel.StoreProductId))
                .FirstOrDefaultAsync();

            if (entity != null)
            {
                if (updateModel.QtyHand != null)
                {
                    //QtyHandTask
                    if (entity.QtyHand != updateModel.QtyHand.Value)
                    {
                        await _qtyRepository.CreateQtyHandTrack(entity.Product, entity.Store, entity.QtyHand,
                            updateModel.QtyHand.Value);
                    }

                    entity.QtyHand = updateModel.QtyHand.Value;
                }

                if (updateModel.OrderTrigger != null)
                {
                    entity.OrderTrigger = updateModel.OrderTrigger.Value;
                }

                if (updateModel.OrderAmount != null)
                {
                    entity.OrderAmount = updateModel.OrderAmount.Value;
                }

                if (updateModel.BinLocationId <= 0)
                {
                    entity.BinLocationId = null;
                }
                else
                {
                    entity.BinLocationId = updateModel.BinLocationId;
                }

                await _repository.UpdateAsync(updateModel.StoreProductId, entity);

                return entity;
            }

            return null;
        }

        /// <summary>
        ///     Get all entities
        /// </summary>
        /// <returns></returns>
        public override async Task<List<StoreProduct>> Get()
        {
            var entities = await _repository.GetEntity<StoreProduct>()
                .Include(x => x.Store)
                .Include(x => x.Taxs)
                //.Include(x => x.Discounts)
                .Include(x => x.Product.Category)
                .Include(x => x.Product.Family)

                //.Include(x => x.Product.KitProducts)
                //.Include("Product.Products")
                .Include("Product.ScaleCategory")
                // .Include("Product.ScaleLabelTypes")
                .Include("Product.VendorProducts")
                .ToListAsync();
            return entities;
        }

        /// <summary>
        ///     Get all entities
        /// </summary>
        /// <returns></returns>
        public async Task<StoreProduct> GetWithProduct(long productId)
        {
            var entities = await _repository.GetEntity<StoreProduct>()
                .Include(x => x.Store)
                .Include(x => x.Product)
                .FirstOrDefaultAsync(x => x.Id == productId);
            return entities;
        }

        public void UpdateValues(StoreProduct entity)
        {
            if( entity.TargetGrossProfit > 0) 
                UpdateValuesByTargetGrossProfit(entity, entity.TargetGrossProfit);
            else
                UpdateValuesNormal(entity);
        }

        public void UpdateValuesNormal(StoreProduct entity)
        {
            if (entity.Cost == 0)
                entity.GrossProfit = 100;
            else
            {
                var grossProfit = (((entity.Price * 100 - entity.Cost * 100) / entity.Price) * 100) / 100;
                entity.GrossProfit = /*(grossProfit< 0)? 0:*/ grossProfit;
            }
            entity.TargetOldPrice = entity.Price;
        }

        /// <inheritdoc />
        public void UpdateValuesByTargetGrossProfit(StoreProduct entity, decimal targetGrossProfit)
        {
            //  GrossProfit = (( Price - Cost ) * 100)/ Price;
            //  Price = ( 100 * Cost )/( 100 - GrossProfit );

            //Update storeProduct price
            if (targetGrossProfit == 100)
                entity.Price = 100 * entity.Cost;
            else
            {
                entity.Price = (100 * entity.Cost) / (100 - targetGrossProfit);
            }

            //Rounded storeProductPrice
            var stringPrice = entity.Price.ToString("N2");
            var lastDigit = int.Parse(stringPrice[^1].ToString());
            var endDigit = _roundingTableService.ChangeBy(lastDigit).Result;
            entity.Price = (endDigit < 0) ? entity.Price : decimal.Parse($"{stringPrice[..^1]}{endDigit}");
            //entity.Price2 = entity.WebPrice = entity.Price;

            //Update grossProfit
            if (entity.Cost == 0)
                entity.GrossProfit = 100;
            else
            {
                var grossProfit = (((entity.Price * 100 - entity.Cost * 100) / entity.Price) * 100) / 100;
                entity.GrossProfit = /*(grossProfit< 0)? 0:*/ grossProfit;
            }
            //entity.GrossProfit2 = entity.WebGrossProfit = entity.GrossProfit;
        }

        /// <summary>
        ///     Insert a entity
        /// </summary>
        /// <param name="entity">Entity to insert</param>
        /// <returns>Entity</returns>
        public override async Task<StoreProduct> Post(StoreProduct entity)
        {
            var data = await _repository.TransactionAsync(async context =>
            {
                if (entity.Taxs != null)
                {
                    //toSynchro.Taxs = new List<Tax>();
                    for (var i = 0; i < entity.Taxs.Count; i++)
                    {
                        //toSynchro.Taxs.Add(new Tax {Id = entity.Taxs[i].Id});
                        _repository.GetEntity<Tax>().Attach(entity.Taxs[i]);
                    }
                }

                //Recalculating the Price, GrossProfit
                UpdateValues(entity);

                var id = await _repository.CreateAsync(entity);
                entity.Id = id;

                return entity;
            });

            var elem = await _repository.GetEntity<StoreProduct>()
                .Where(x => x.Id == data.Id)
                .Include(x => x.Product)
                .FirstOrDefaultAsync();

            var toSyn = elem.Product.ProductType switch
            {
                ProductType.SPT => LiteProduct.Convert(elem.Product, elem),
                ProductType.SLP => LiteScaleProduct.Convert1((ScaleProduct)elem.Product, elem),
                _ => LiteProduct.Convert(elem.Product, elem),
            };

            // public product created event

            await synchroService.AddSynchroToStore(
                entity.StoreId,
                toSyn,
                SynchroType.CREATE
            );

            if (elem.Product.AddOnlineStore)
            {
                await _mediator.Publish(new ProductCreated(
                    toSyn, 
                    new List<long>() { entity.StoreId }, 
                    elem.Id)).ConfigureAwait(false);
            }

            await _shelfTagService.PostFromStoreProduct(elem.Id);

            return data;
        }

        /// <summary>
        ///     Insert a entity
        /// </summary>
        /// <param name="entity">Entity to insert</param>
        /// <param name="product"></param>
        /// <returns>Entity</returns>
        public async Task<StoreProduct> PostImport(StoreProduct entity, Product product)
        {
            // return await this._repository.TransactionAsync<StoreProduct>(async context =>
            // {
            //var toSynchro = new StoreProduct
            //{
            //    ProductId = entity.ProductId,
            //    StoreId = entity.StoreId,
            //    Price = entity.Price,
            //    Cost = entity.Cost,
            //    BinLocation = entity.BinLocation,
            //    GrossProfit = entity.GrossProfit
            //};


            if (entity.Taxs != null)
            {
                //toSynchro.Taxs = new List<Tax>();
                for (var i = 0; i < entity.Taxs.Count; i++)
                {
                    //toSynchro.Taxs.Add(new Tax {Id = entity.Taxs[i].Id});
                    _repository.GetEntity<Tax>().Attach(entity.Taxs[i]);
                }
            }

            var id = await _repository.CreateAsync(entity);
            entity.Id = id;
            //toSynchro.Id = id;

            //toSynchro.Product = product;
            //toSynchro.Product.StoreProducts = null;

            //await synchroService.AddSynchroToStores(
            //    new List<long> {entity.StoreId},
            //    toSynchro,
            //    SynchroType.CREATE);

            var elem = await _repository.GetEntity<StoreProduct>()
                .Where(x => x.Id == id)
                .Include(x => x.Product)
                .FirstOrDefaultAsync();

            var toSyn =
                elem.Product.ProductType == ProductType.SPT
                    ? LiteProduct.Convert(elem.Product, elem)
                    :
                    elem.Product.ProductType == ProductType.SLP
                        ?
                        LiteScaleProduct.Convert1((ScaleProduct)elem.Product, elem)
                        :
                        LiteKitProduct.Convert2((KitProduct)elem.Product, elem);

            // public product created event

            await synchroService.AddSynchroToStore(
                entity.StoreId,
                toSyn,
                SynchroType.CREATE
            );

            if (elem.Product.AddOnlineStore)
            {
                await _mediator.Publish(new ProductCreated(
                    toSyn, 
                    new List<long>() { entity.StoreId }, 
                    elem.Id));
            }
            

            return entity;
            // });
        }

        public async Task<bool> CreateOnMultipleStores(List<long> stores, StoreProduct entity)
        {
            List<long> spIds = new List<long>();

            foreach (var e in stores)
            // stores.ForEach(async e =>
            {
                //Recalculating the Price, GrossProfit
                UpdateValues(entity);

                var id = await _repository.CreateAsync(new StoreProduct
                {
                    ProductId = entity.ProductId,
                    StoreId = e,
                    Price = entity.Price,
                    Price2 = entity.Price2,
                    WebPrice = entity.WebPrice,
                    Cost = entity.Cost,
                    // Location = entity.Location,
                    GrossProfit = entity.GrossProfit,
                    GrossProfit2 = entity.GrossProfit2,
                    WebGrossProfit = entity.WebGrossProfit,
                    NoCategoryChange = entity.NoCategoryChange
                });

                spIds.Add(id);

                var elem = await _repository.GetEntity<StoreProduct>()
                    .Where(x => x.Id == id)
                    .Include(x => x.Product)
                    .FirstOrDefaultAsync();

                var toSyn = elem.Product.ProductType switch
                {
                    ProductType.SPT => LiteProduct.Convert(elem.Product, elem),
                    ProductType.SLP => LiteScaleProduct.Convert1((ScaleProduct)elem.Product, elem),
                    _ => LiteProduct.Convert(elem.Product, elem),
                };
                // public product created event

                await synchroService.AddSynchroToStore(
                    e,
                    toSyn,
                    SynchroType.CREATE
                );

                if (elem.Product.AddOnlineStore)
                {                    
                    await _mediator.Publish(new ProductCreated(
                        toSyn, 
                        new List<long>() { e }, 
                        elem.Id));
                }
                
            }

            foreach (var spId in spIds)
            {
                await _shelfTagService.PostFromStoreProduct(spId);
            }

            return true;
        }

        /// <summary>
        ///     Update entity
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="entity">Entity to update</param>
        /// <returns>Boolean success</returns>
        /// <exception cref="BusinessLogicException">If id is less to -1</exception>
        public override async Task<bool> Put(long id, StoreProduct entity)
        {
            if (id < 1)
            {
                _logger.LogError("Id parameter out of bounds");
                throw new BusinessLogicException("Id parameter out of bounds.");
            }


            // remove all stores first
            var sP = await _repository.GetEntity<StoreProduct>()
                .Include(x => x.Taxs)
                .Include(x => x.Product)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            var oldDepartment = sP.Product.DepartmentId;
            var oldCategory = sP.Product.CategoryId;

            var longList = entity.Taxs.Select(x => x.Id).ToList();
            var removeList = new List<long>();
            var addList = new List<long>();
            var updateList = new List<long>();
            foreach (var store in sP.Taxs.ToList())
                // Remove the roles which are not in the list of new roles
                if (!longList.Contains(store.Id))
                {
                    sP.Taxs.Remove(store);
                    removeList.Add(store.Id);
                }

            foreach (var newStoreId in longList)
                // Add the roles which are not in the list of user's roles
                if (!sP.Taxs.Any(r => r.Id == newStoreId))
                {
                    addList.Add(newStoreId);
                    var newEnt = new Tax { Id = newStoreId };
                    _repository.GetEntity<Tax>().Attach(newEnt);
                    sP.Taxs.Add(newEnt);
                }
                else
                {
                    updateList.Add(newStoreId);
                }

            entity.Taxs.Clear();

            // await this.synchroService.AddSynchroToStores<Tax>(addList, entity, Api.Entities.Enum.SynchroType.CREATE);
            // await this.synchroService.AddSynchroToStores<Tax>(updateList, entity, Api.Entities.Enum.SynchroType.UPDATE);
            // await this.synchroService.AddSynchroToStores<Tax>(removeList, entity, Api.Entities.Enum.SynchroType.DELETE);

            entity.Taxs = sP.Taxs;

            //Recalculating the Price, GrossProfit
            UpdateValues(entity);


            bool needAddShelfTag = (entity.Price != sP.Price);

            var res = await _repository.UpdateAsync(id, entity);
            if (res)
            {
                var elem = await _repository.GetEntity<StoreProduct>()
                    .Where(x => x.Id == id)
                    .Include(x => x.Product)
                    .FirstOrDefaultAsync();

                var toSyn =
                    elem.Product.ProductType == ProductType.SPT
                        ? LiteProduct.Convert(elem.Product, elem)
                        :
                        elem.Product.ProductType == ProductType.SLP
                            ?
                            LiteScaleProduct.Convert1((ScaleProduct)elem.Product, elem)
                            :
                            LiteKitProduct.Convert2((KitProduct)elem.Product, elem);

                // public product created event
                await _mediator.Publish(new ProductUpdated(
                    toSyn, 
                    new List<long>() { entity.StoreId }, 
                    oldDepartment, 
                    oldCategory, 
                    elem.Id));

                await synchroService.AddSynchroToStore(
                    entity.StoreId,
                    toSyn,
                    SynchroType.UPDATE
                );

                if (needAddShelfTag)
                    await _shelfTagService.PostFromStoreProduct(id);
            }

            return res;
        }

        public async Task<StoreProduct> GetAllByProductAndStore(long productId, long storeId)
        {
            var entities = await _repository.GetEntity<StoreProduct>()
                .Include(e => e.Product)
                .Include(x => x.Product.DefaulShelfTag)
                .Include(sp => sp.Product.Category)
                .Include(sp => sp.Product.Department)
                .Include(sp => sp.Store)
                .Include(x => x.Child)
                .Where(x => x.ProductId == productId && x.StoreId == storeId)
                .FirstOrDefaultAsync();
            return entities;
        }

        /// <summary>
        ///     Get all entities
        /// </summary>
        /// <returns></returns>
        public async Task<List<StoreProduct>> GetAllByProduct(long productId)
        {
            var entities = await _repository.GetEntity<StoreProduct>()
                .Include(x => x.Store)
                .Include(x => x.Taxs)
                // .Include(x => x.Discounts)
                // .Include(x => x.Product.Category)
                // .Include(x => x.Product.Family)
                // .Include(x => x.Product.ProductImages)

                //.Include(x => x.Product.KitProducts)
                //.Include("Product.Products")
                // .Include("Product.ScaleCategory")
                // .Include("Product.ScaleLabelTypes")
                // .Include("Product.VendorProducts")
                .Where(x => x.ProductId == productId)
                .ToListAsync();
            return entities;
        }

        protected override IQueryable<StoreProduct> FilterqueryBuilder(
            StoreProduct filter,
            string searchstring,
            string[] splited,
            DbSet<StoreProduct> query)
        {
            IQueryable<StoreProduct> query1 = null;

            if (!string.IsNullOrEmpty(searchstring))
                query1 = query.Where(c => c.Store.Name.Contains(searchstring));
            else
                query1 = query.WhereIf(filter.StoreId > 0, c => c.StoreId == filter.StoreId);

            query1 = query1
                .Switch(splited)
                .OrderByCase(e => e[0] == "name" && e[1] == "asc", e => e.Store.Name)
                .OrderByDescendingCase(e => e[0] == "name" && e[1] == "desc", e => e.Store.Name)
                .OrderByDefault(e => e.Store.Name);

            return query1.Include(x => x.Store)
                //.Include(x => x.Taxs).IgnoreAutoIncludes()
                .Include(x => x.Product)
                .Include(x => x.BinLocation)
                // .Select(x => new StoreProduct()
                // {
                //     Id = x.Id,
                //     Product = x.Product == null ? null : new Product()
                //     {
                //         Id = x.Product.Id,
                //         UPC = x.Product.UPC,
                //         Name = x.Product.Name
                //     },
                //     BinLocation = x.BinLocation == null ? null : new BinLocation()
                //     {
                //         Id = x.BinLocation.Id,
                //         Name = x.BinLocation.Name
                //     },
                //     QtyHand = x.QtyHand,
                //     OrderTrigger = x.OrderTrigger,
                //     OrderAmount = x.OrderAmount
                // })
                .Where(x => x.ProductId == filter.ProductId);
        }

        public override async Task<bool> Delete(long id)
        {
            var elem = await _repository.GetEntity<StoreProduct>()
                .Where(x => x.Id == id)
                .Include(x => x.Product)
                .FirstOrDefaultAsync();

            var toSyn =
                elem.Product.ProductType == ProductType.SPT
                    ? LiteProduct.Convert(elem.Product, elem)
                    :
                    elem.Product.ProductType == ProductType.SLP
                        ?
                        LiteScaleProduct.Convert1((ScaleProduct)elem.Product, elem)
                        :
                        LiteKitProduct.Convert2((KitProduct)elem.Product, elem);

            // public product created event
            await _mediator.Publish(new ProductDeleted(toSyn, new List<long>() { elem.StoreId }, elem.Id));
            await synchroService.AddSynchroToStore(
                elem.StoreId,
                toSyn,
                SynchroType.DELETE
            );
            return await base.Delete(id);
        }

        /// <summary>
        ///     Update entity
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="orderAmount"></param>
        /// <returns>Boolean success</returns>
        /// <exception cref="BusinessLogicException">If id is less to -1</exception>
        public async Task<bool> PutByOrderAmount(long id, decimal orderAmount)
        {
            if (id < 1)
            {
                _logger.LogError("Id parameter out of bounds");
                throw new BusinessLogicException("Id parameter out of bounds");
            }

            return await _repository.TransactionAsync(async context =>
            {
                var suggested = await _repository.GetEntity<StoreProduct>()
                    .WithSpecification(new GetByIdSpec<StoreProduct>(id))
                    .FirstOrDefaultAsync();

                if (suggested != null)
                {
                    suggested.OrderAmount = orderAmount;

                    var response = await _repository.UpdateAsync(id, suggested);

                    var data = await _repository.GetEntity<StoreProduct>()
                        .WithSpecification(new GetByIdSpec<StoreProduct>(id))
                        .FirstOrDefaultAsync();

                    return response;
                }

                return false;
            });
        }
    }
}