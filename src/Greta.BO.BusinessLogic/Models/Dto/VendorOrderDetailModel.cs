using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;
using Greta.Sdk.Core.Abstractions;

namespace Greta.BO.BusinessLogic.Models.Dto
{
    public class VendorDetailListModel
    {
        public List<VendorOrderDetailModel> Elements { get; set; }
        
        public long VendorOrder { get; set; }
    }
    public class VendorOrderDetailModel: IDtoLong<string>, IMapFrom<VendorOrderDetail>
    {
        public VendorOrderModel VendorOrder { get; set; }
        [Required]
        public long VendorOrderId { get; set; }
        
        public VendorOrderDetailProductModel Product { get; set; }
        [Required]
        public long ProductId { get; set; }
        
        [Required]
        public decimal QuantityOnHand { get; set; }
        
        public int CasePack { get; set; }
        
        public decimal CaseCost { get; set; }
        [Required]
        public decimal OrderAmount { get; set; }
        
        public decimal RecivedAmount { get; set; }
        [Required]
        public string ProductCode { get; set; }
        [Required]
        public string UPC { get; set; }
        
        public decimal Price { get; set; }
        public decimal Cost { get; set; }
        public decimal GrossProfit { get; set; }
        
        public string PackSize { get; set; }
        
        public long Id { get; set; }

        public bool State { get; set; }
        public string UserCreatorId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class VendorOrderDetailProductModel: IMapFrom<Product>
    {
        public long Id { get; set; }
        public string UPC { get; set; }
        public string Name { get; set; }
        public List<VendorOrderDetailVendorProductModel> VendorProducts { get; set; }
    }
    
    public class VendorOrderDetailVendorProductModel : IMapFrom<VendorProduct>
    {
        [Required]
        [StringLength(24, ErrorMessage = "The {0} field not is valid")]
        public string ProductCode { get; set; }

        public bool IsPrimary { get; set; }

        [Required] public int CasePack { get; set; }

        [Required] public decimal CaseCost { get; set; }

        [Required] public decimal UnitCost { get; set; }

        [Required] public VendorProductType OrderByCase { get; set; }

        public DateTime? LastOrderDate { get; set; }

        public long VendorId { get; set; }
        public long ProductId { get; set; }

        public long Id { get; set; }
    }
    
    public class VendorDetailReceivedListModel
    {
        public List<VendorOrderReceivedDetailModel> Elements { get; set; }
    }
    public class VendorOrderReceivedDetailModel: IMapFrom<VendorOrderDetail>
    {
        [Required]
        public long VendorOrderDetailId { get; set; }
        public decimal ReceivedAmount { get; set; }
        public int CasePack { get; set; }
        public decimal CaseCost { get; set; }
        
        public decimal Price { get; set; }
        public decimal Cost { get; set; }
        public decimal GrossProfit { get; set; }
        
        public string PackSize { get; set; }
        public List<VendorOrderReceivedDetailCreditModel> Credits { get; set; }
    }
    
    public class VendorOrderReceivedDetailCreditModel//: IMapFrom<VendorOrderDetail>
    {
        public int CreditReason { get; set; }
        
        public decimal Cost { get; set; }
        
        public decimal Quantity { get; set; }
        
        public bool Unit { get; set; }
        public decimal CreditAmount { get; set; }
    }
    
    
}