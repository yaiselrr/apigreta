using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.Specification;
using Greta.BO.Api.Abstractions;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.BusinessLogic.Exceptions;
using Greta.BO.BusinessLogic.Interfaces;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Models.Dto.Search;
using Greta.Sdk.Core.Models.Pager;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Greta.BO.BusinessLogic.Service
{
    /// <summary>
    /// Service layer for vendor order entity
    /// </summary>
    public interface IVendorOrderService : IGenericBaseService<VendorOrder>
    {
        /// <summary>
        /// Get Vendor order with details
        /// </summary>
        /// <param name="id">VendorOrder Id</param>
        /// <returns>Return a Vendor Order with details</returns>
        Task<VendorOrder> GetWithDetails(long id);
        
        /// <summary>
        /// Get Vendor Order with details, vendor and store
        /// </summary>
        /// <param name="id">VendorOrder Id</param>
        /// <returns>Return a Vendor Order with details, vendor and store</returns>
        Task<VendorOrder> GetFullOrder(long id);
        
        /// <summary>
        /// Determine if the vendor order have any product
        /// </summary>
        /// <param name="vendorOrderId">VendorOrder Id</param>
        /// <returns>Return true if any detail of vendor order has products associated</returns>
        Task<bool> HasProducts(long vendorOrderId);

        /// <summary>
        /// Get only details with the receive data necesary for call the receive on DSD functionality
        /// </summary>
        /// <param name="vendorOrderId"></param>
        /// <returns></returns>
        Task<List<VendorOrderReceivedDetailModel>> GetOnlyReceivedModeDetails(long vendorOrderId);

    }

    /// <inheritdoc cref="Greta.BO.BusinessLogic.Service.IVendorOrderService"/>
    public sealed class VendorOrderService : BaseService<IVendorOrderRepository, VendorOrder>, IVendorOrderService
    {
        /// <inheritdoc />
        public VendorOrderService(
            IVendorOrderRepository repository,
            ILogger<VendorOrderService> logger)
            : base(repository, logger)
        {
        }
        
        /// <inheritdoc cref="IGenericBaseService{T}.FilterSpec" />
        public override async Task<Pager<VendorOrder>> FilterSpec(
            int currentPage,
            int pageSize,
            ISpecification<VendorOrder> spec,
            CancellationToken cancellationToken = default)
        {
            return await _repository.PagesAsync(spec, currentPage, pageSize, cancellationToken: cancellationToken);
        }


        /// <inheritdoc />
        public async Task<VendorOrder> GetWithDetails(long id)
        {
            if (id < 1)
            {
                _logger.LogError("Id parameter out of bounds");
                throw new BusinessLogicException("Id parameter out of bounds.");
            }

            var entity = await _repository.GetEntity<VendorOrder>()
                .Include(v => v.VendorOrderDetails)
                .FirstOrDefaultAsync(v => v.Id == id);

            return entity;
        }
        
        /// <inheritdoc />
        public async Task<List<VendorOrderReceivedDetailModel>> GetOnlyReceivedModeDetails(long vendorOrderId)
        {
            var entity = await _repository.GetEntity<VendorOrderDetail>()
                .Where(v => v.VendorOrderId == vendorOrderId)
                .Select(x => new VendorOrderReceivedDetailModel
                {
                    VendorOrderDetailId = x.Id,
                    ReceivedAmount = x.RecivedAmount,
                    CasePack = x.CasePack,
                    CaseCost = x.CaseCost,
                    PackSize = x.PackSize,
                    Credits = null,
                    
                    Cost = x.Cost,
                    Price = x.Price,
                    GrossProfit = x.GrossProfit
                })
                .ToListAsync();

            return entity;
        }

        /// <inheritdoc />
        public async Task<VendorOrder> GetFullOrder(long id)
        {
            if (id < 1)
            {
                _logger.LogError("Id parameter out of bounds");
                throw new BusinessLogicException("Id parameter out of bounds.");
            }

            var entity = await _repository.GetEntity<VendorOrder>()
                .Include(v => v.VendorOrderDetails).ThenInclude(x => x.Product)
                .Include(x => x.Vendor).ThenInclude(x => x.VendorContacts)
                .Include(x => x.Store)
                .FirstOrDefaultAsync(v => v.Id == id);

            return entity;
        }

        /// <inheritdoc />
        public async Task<bool> HasProducts(long vendorOrderId)
        {
            var vendorOrder = await _repository.GetEntity<VendorOrder>()
                .Include(x => x.VendorOrderDetails)
                .Where(x => (x.Id == vendorOrderId))
                .FirstOrDefaultAsync();
            return vendorOrder?.VendorOrderDetails is { Count: > 0 };
        }
        
        
    }

    /// <summary>
    /// Service layer for vendor order detail entity
    /// </summary>
    public interface IVendorOrderDetailService : IGenericBaseService<VendorOrderDetail>
    {
        /// <summary>
        /// List of vendor order detail of a vendor order
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>Return a vendor order detail list from a vendor order</returns>
        Task<List<VendorOrderDetail>> FilterCustom(VendorOrderDetailSearchModel filter);
        
        /// <summary>
        /// Save list of vendor order detail and assign to a vendor order
        /// </summary>
        /// <param name="entity">List of vendor order detail to save</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> PostMultiple(List<VendorOrderDetail> entity, CancellationToken cancellationToken);
        
        /// <summary>
        /// Get storeProduct with these storeId, upc and vendorId 
        /// </summary>
        /// <param name="storeId"> Store Id</param>
        /// <param name="vendorId">Vendor Id</param>
        /// <param name="upc">UPC</param>
        /// <returns>Return null if no exist product with these params, else return the storeProduct </returns>
        Task<StoreProduct> GetStoreProductByUpc(long storeId, long vendorId, string upc);
        
        /// <summary>
        /// Get the status of the vendor order
        /// </summary>
        /// <param name="vendorOrderId">Vendor Order Id</param>
        /// <returns></returns>
        Task<int> GetStatusPurchaseOrder(long vendorOrderId);
    }

    /// <inheritdoc cref="Greta.BO.BusinessLogic.Service.IVendorOrderDetailService"/>
    public class VendorOrderDetailService : BaseService<IVendorOrderDetailRepository, VendorOrderDetail>,
        IVendorOrderDetailService
    {
        /// <inheritdoc />
        public VendorOrderDetailService(
            IVendorOrderDetailRepository repository,
            ILogger<VendorOrderDetailService> logger)
            : base(repository, logger)
        {
        }

        /// <inheritdoc />
        public async Task<List<VendorOrderDetail>> FilterCustom(
            VendorOrderDetailSearchModel filter)
        {
            var splited = string.IsNullOrEmpty(filter.Sort) ? new[] { "", "" } : filter.Sort.Split("_");

            var query = _repository.GetEntity<VendorOrderDetail>();
            IQueryable<VendorOrderDetail> query1; //FilterqueryBuilder(filter, searchstring, splited, query);
            if (!string.IsNullOrEmpty(filter.Search))
            {
                query1 = query.Where(c => c.Product.Name.Contains(filter.Search));
            }
            else
            {
                query1 = query
                    .WhereIf(!string.IsNullOrEmpty(filter.ProductName),
                        c => c.Product.Name.Contains(filter.ProductName))
                    .WhereIf(filter.HeaderId > 0, c => c.VendorOrderId == filter.HeaderId)
                    .WhereIf(filter.StoreId > 0, c => c.VendorOrder.StoreId == filter.StoreId);
            }

            var vendorId = await _repository.GetEntity<VendorOrder>()
                .Where(vo => vo.Id == filter.HeaderId)
                .Select(x => x.VendorId).FirstOrDefaultAsync();

            query1 = query1
                .Switch(splited)
                .OrderByCase(e => e[0] == "product" && e[1] == "asc", e => e.Product.Name)
                .OrderByDescendingCase(e => e[0] == "product" && e[1] == "desc", e => e.Product.Name)
                .OrderByCase(e => e[0] == "quantityOnHand" && e[1] == "asc", e => e.QuantityOnHand)
                .OrderByDescendingCase(e => e[0] == "quantityOnHand" && e[1] == "desc", e => e.QuantityOnHand)
                .OrderByCase(e => e[0] == "orderAmount" && e[1] == "asc", e => e.OrderAmount)
                .OrderByDescendingCase(e => e[0] == "orderAmount" && e[1] == "desc", e => e.OrderAmount)
                .OrderByDefault(e => e.Id);

            var entities = await query1
                .Include(x => x.Product)
                .ThenInclude(p => p.VendorProducts.Where(vp => vp.VendorId == vendorId))
                .ToListAsync();
            return entities;
        }

        /// <inheritdoc />
        public async Task<bool> PostMultiple(List<VendorOrderDetail> entity, CancellationToken cancellationToken)
        {
            var result = await _repository.CreateRangeAsync(entity, cancellationToken);
            return result.Count == entity.Count;
        }

        /// <inheritdoc />
        public async Task<StoreProduct> GetStoreProductByUpc(long storeId, long vendorId, string upc)
        {
            try
            {
                var upcNew = (upc.Length == 13) ? upc.Substring(0, 12) : upc;

                var prods = await _repository.GetEntity<Product>()
                    .Include(e => e.StoreProducts.Where(vp => vp.StoreId == storeId))
                    .Include(x => x.VendorProducts.Where(vp => vp.VendorId == vendorId))
                    .Where(x => (x.UPC == upcNew || x.UPC == upc || x.VendorProducts.Any(p => p.ProductCode == upc)))
                    .FirstOrDefaultAsync();

                if (prods == null || prods.StoreProducts.Count == 0 || prods.VendorProducts.Count == 0)
                {
                    return null;
                }

                var result = new StoreProduct()
                {
                    Product = prods,
                    Id = prods.Id,
                    QtyHand = prods.StoreProducts[0].QtyHand,
                    OrderAmount = prods.StoreProducts[0].OrderAmount,
                    Cost = prods.StoreProducts[0].Cost,
                    Price = prods.StoreProducts[0].Price,
                    GrossProfit = prods.StoreProducts[0].GrossProfit,
                    NoCategoryChange = prods.StoreProducts[0].NoCategoryChange,
                    TargetGrossProfit = prods.StoreProducts[0].TargetGrossProfit
                };
                return result;
            }
            catch
            {
                return null;
            }
        }

        /// <inheritdoc />
        public async Task<int> GetStatusPurchaseOrder(long vendorOrderId)
        {
            VendorOrderStatus vos = await _repository.GetEntity<VendorOrder>()
                .Where(x => (x.Id == vendorOrderId))
                .Select(x => x.Status)
                .FirstOrDefaultAsync();
            return (int)vos;
        }
    }

    /// <summary>
    /// Service layer for vendor order detail credit entity
    /// </summary>
    public interface IVendorOrderDetailCreditService : IGenericBaseService<VendorOrderDetailCredit>
    {
        /// <summary>
        /// List of vendor order detail credit of a vendor order
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>Return a vendor order detail credit list from a vendor order</returns>
        Task<List<VendorOrderDetailCredit>> FilterCustom(VendorOrderDetailCreditSearchModel filter);
        
        /// <summary>
        /// Get a vendor order credit list from a Vendor order
        /// </summary>
        /// <param name="id">Vendor Order Id</param>
        /// <returns>Return a vendor order credit list </returns>
        Task<List<VendorOrderDetailCredit>> GetCreditsByVendorOrder(long id);
    }

    /// <inheritdoc cref="Greta.BO.BusinessLogic.Service.IVendorOrderDetailCreditService"/>
    public class VendorOrderDetailCreditService :
        BaseService<IVendorOrderDetailCreditRepository, VendorOrderDetailCredit>,
        IVendorOrderDetailCreditService
    {
        /// <inheritdoc />
        public VendorOrderDetailCreditService(
            IVendorOrderDetailCreditRepository repository,
            ILogger<VendorOrderDetailCreditService> logger)
            : base(repository, logger)
        {
        }

        /// <inheritdoc />
        public async Task<List<VendorOrderDetailCredit>> FilterCustom(VendorOrderDetailCreditSearchModel filter)
        {
            var splited = string.IsNullOrEmpty(filter.Sort) ? new[] { "", "" } : filter.Sort.Split("_");

            var query = _repository.GetEntity<VendorOrderDetailCredit>();
            IQueryable<VendorOrderDetailCredit> query1 = query;
            query1 = query1
                .Switch(splited)
                .OrderByDefault(e => e.Id);

            var entities = await query1
                .Include(x => x.VendorOrderDetail)
                .Where(x => x.VendorOrderDetail.VendorOrderId == filter.HeaderId)
                .ToListAsync();
            return entities;
        }

        /// <inheritdoc />
        public async Task<List<VendorOrderDetailCredit>> GetCreditsByVendorOrder(long id)
        {
            if (id < 1)
            {
                _logger.LogError("Id parameter out of bounds");
                throw new BusinessLogicException("Id parameter out of bounds.");
            }

            return await _repository.GetEntity<VendorOrderDetailCredit>()
                .Include(v => v.VendorOrderDetail)
                .Where(v => v.VendorOrderDetail.VendorOrderId == id)
                .ToListAsync();
        }
    }
}