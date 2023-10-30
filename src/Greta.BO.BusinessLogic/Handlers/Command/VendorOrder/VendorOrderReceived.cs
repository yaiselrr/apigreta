using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Greta.BO.Api.Abstractions;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.BusinessLogic.Authorization;
using Greta.BO.BusinessLogic.Authorization.Requirements;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.BO.BusinessLogic.Service;
using MediatR;

namespace Greta.BO.BusinessLogic.Handlers.Command.VendorOrder;


/// <summary>
/// Command to receive vendor order
/// </summary>
/// <param name="EntityId"></param>
/// <param name="Entity"></param>
public record VendorOrderReceivedCommand(long EntityId, VendorDetailReceivedListModel Entity) : IRequest<VendorOrderReceivedResponse>,
    IAuthorizable
{
    /// <inheritdoc />
    public List<IRequirement> Requirements => new()
    {
        new PermissionRequirement.Requirement($"add_edit_vendor_order")
    };
}

/// <inheritdoc />
public class VendorOrderReceivedHandler : IRequestHandler<VendorOrderReceivedCommand, VendorOrderReceivedResponse>
{
    private readonly IVendorOrderService _service;
    private readonly IVendorOrderDetailService _serviceDetail;
    private readonly IVendorOrderDetailCreditService _serviceDetailCredit;
    private readonly IStoreProductService _serviceStoreProduct;
    private readonly IVendorProductService _serviceVendorProduct;
    private readonly IProductService _serviceProduct;
    private readonly IVendorService _serviceVendor;
    private readonly IQtyHandTrackRepository _qtyHandTrackRepository;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
    /// <param name="serviceDetail"></param>
    /// <param name="serviceDetailCredit"></param>
    /// <param name="serviceStoreProduct"></param>
    /// <param name="serviceVendorProduct"></param>
    /// <param name="qtyHandTrackRepository"></param>
    /// <param name="serviceProduct"></param>
    /// <param name="serviceVendor"></param>
    public VendorOrderReceivedHandler(
        IVendorOrderService service,
        IVendorOrderDetailService serviceDetail,
        IVendorOrderDetailCreditService serviceDetailCredit,
        IStoreProductService serviceStoreProduct,
        IVendorProductService serviceVendorProduct,
        IQtyHandTrackRepository qtyHandTrackRepository,
        IProductService serviceProduct,
        IVendorService serviceVendor)
    {
        _service = service;
        _serviceDetail = serviceDetail;
        _serviceDetailCredit = serviceDetailCredit;
        _serviceStoreProduct = serviceStoreProduct;
        _serviceVendorProduct = serviceVendorProduct;
        _qtyHandTrackRepository = qtyHandTrackRepository;
        _serviceProduct = serviceProduct;
        _serviceVendor = serviceVendor;
    }

    /// <inheritdoc />
    public async Task<VendorOrderReceivedResponse> Handle(VendorOrderReceivedCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var order = await _service.Get(request.EntityId);

            foreach (var elem in request.Entity.Elements)
            {
                //VENDOR ORDER DETAIL
                var orderDetail = await _serviceDetail.Get(elem.VendorOrderDetailId);
                orderDetail.RecivedAmount = elem.ReceivedAmount;
                orderDetail.CaseCost = elem.CaseCost;
                orderDetail.CasePack = elem.CasePack;
                if(elem.PackSize != null)
                    orderDetail.PackSize = elem.PackSize;
                // if (elem.CasePack > 1)
                // {
                //     orderDetail.CaseCost /= elem.CasePack;
                // }

                //VENDOR PRODUCT
                var vendorProduct =
                    await _serviceVendorProduct.GetAllByVendorAndProduct(order.VendorId, orderDetail.ProductId);
                var unitCost = (elem.CasePack != 0)
                    ? elem.CaseCost / elem.CasePack
                    : 0;
                if (vendorProduct != null &&
                    (elem.CaseCost != vendorProduct.CaseCost || elem.CasePack != vendorProduct.CasePack || elem.PackSize != null))
                {
                    if (elem.CaseCost != vendorProduct.CaseCost)
                        vendorProduct.CaseCost = elem.CaseCost;

                    if (elem.CasePack != vendorProduct.CasePack)
                        vendorProduct.CasePack = elem.CasePack;

                    vendorProduct.UnitCost = unitCost;
                    
                    if (elem.PackSize != null)
                        vendorProduct.PackSize = elem.PackSize;

                    await _serviceVendorProduct.Put(vendorProduct.Id, vendorProduct);
                }

                if (elem.ReceivedAmount > 0)
                {
                    var storeProduct = await _serviceStoreProduct.GetAllByProductAndStore(
                        orderDetail.ProductId, order.StoreId);

                    if (storeProduct != null)
                    {
                        if (storeProduct.Cost != unitCost)
                        {
                            storeProduct.Cost = unitCost;
                            storeProduct.Price = elem.Price; 
                            _serviceStoreProduct.UpdateValues(storeProduct);

                            orderDetail.Price = storeProduct.Price;
                            orderDetail.Cost = storeProduct.Cost;
                            orderDetail.GrossProfit = storeProduct.GrossProfit;
                        }

                        if (storeProduct.QtyHand < 0)
                        {
                            await _qtyHandTrackRepository.CreateQtyHandTrack(storeProduct.Product, storeProduct.Store,
                                storeProduct.QtyHand,
                                0, cancellationToken);
                            storeProduct.QtyHand = 0;
                        }

                        await _qtyHandTrackRepository.CreateQtyHandTrack(storeProduct.Product, storeProduct.Store,
                            storeProduct.QtyHand,
                            storeProduct.QtyHand + (elem.CasePack * elem.ReceivedAmount), cancellationToken);
                        storeProduct.QtyHand += elem.CasePack * elem.ReceivedAmount;
                        
                        await _serviceStoreProduct.Put(storeProduct.Id, storeProduct);
                        
                        if (storeProduct.Child != null)
                        {
                            await UpdateCostOnChilds(storeProduct.Child.Id, storeProduct.Cost);
                        }
                    }
                }

                if (elem.Credits != null)
                {
                    var product = await _serviceProduct.GetProductById(orderDetail.ProductId);
                    var vendor = await _serviceVendor.Get(order.VendorId);

                    foreach (var credit in elem.Credits)
                    {
                        if (vendorProduct != null)
                        {
                            var vodc = new Api.Entities.VendorOrderDetailCredit()
                            {
                                //VendorOrderDetail
                                VendorOrderDetailId = orderDetail.Id,
                                
                                CreditQuantity = credit.Quantity,
                                CreditCost = credit.Cost,
                                CreditAmount = credit.CreditAmount,
                                CreditReason = (CreditReasonType)Enum.ToObject(typeof(CreditReasonType),
                                    credit.CreditReason),
                                IsUnit = credit.Unit,

                                //Product
                                ProductId = orderDetail.ProductId,
                                ProductName = product.Name,
                                ProductCode = orderDetail.ProductCode,
                                ProductUpc = orderDetail.UPC,

                                //Vendor
                                VendorId = order.VendorId,
                                VendorName = vendor.Name,

                                CasePack = orderDetail.CasePack,
                                CaseCost = orderDetail.CaseCost
                            };

                            await _serviceDetailCredit.Post(vodc);
                        }
                    }
                }
                
                await _serviceDetail.Put(orderDetail.Id, orderDetail);
            }

            order.Status = VendorOrderStatus.Received;
            await _service.Put(order.Id, order);

            return new VendorOrderReceivedResponse { Data = true };
        }
        catch (Exception ex)
        {
            return new VendorOrderReceivedResponse { Data = false };
        }
    }

    private async Task UpdateCostOnChilds(long childId, decimal cost)
    {
        var storeProduct = await _serviceStoreProduct.GetWithParent(childId);
        if (storeProduct.SplitCount > 1)
            storeProduct.Cost = cost / storeProduct.SplitCount;
        await _serviceStoreProduct.Put(storeProduct.Id, storeProduct);
        if (storeProduct.Child != null)
        {
            await UpdateCostOnChilds(storeProduct.Child.Id, storeProduct.Cost);
        }
    }
}

/// <inheritdoc />
public record VendorOrderReceivedResponse : CQRSResponse<bool>;