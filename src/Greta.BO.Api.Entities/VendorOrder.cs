using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Greta.BO.Api.Entities.Enum;

namespace Greta.BO.Api.Entities
{
    public class VendorOrder: BaseEntityLong
    {
        public Vendor Vendor { get; set; }
        public long VendorId { get; set; }
        public BOUser User { get; set; }
        public long UserId { get; set; }
        public DateTime ReceivedDate { get; set; }
        public DateTime? OrderedDate { get; set; }
        public Store Store { get; set; }
        public long StoreId { get; set; }
        public string AttachmentFilePath { get; set; }
        
        public string LastEmailId { get; set; }
        
        public int SendCount { get; set; }
        public string InvoiceNumber { get; set; }
        public decimal DeliveryCharge { get; set; }
        public VendorOrderStatus Status { get; set; }
        
        public bool IsDirectStoreDelivery { get; set; }
        public virtual List<VendorOrderDetail> VendorOrderDetails { get; set; }
        //public virtual List<VendorOrderDetailCredit> VendorOrderDetailCredits { get; set; }
    }
    
    public class VendorOrderDetail: BaseEntityLong
    {
        public VendorOrder VendorOrder { get; set; }
        public long VendorOrderId { get; set; }
        public Product Product { get; set; }
        public long ProductId { get; set; }
        public decimal QuantityOnHand { get; set; }
        public string ProductCode { get; set; }
        public string UPC { get; set; }
        public int CasePack { get; set; }
        public decimal OrderAmount { get; set; }
        public decimal RecivedAmount { get; set; }
        public decimal CaseCost { get; set; }
        
        public decimal Price { get; set; }
        public decimal Cost { get; set; }
        public decimal GrossProfit { get; set; }
        
        public string PackSize { get; set; }
        public virtual List<VendorOrderDetailCredit> VendorOrderDetailCredits { get; set; }
    }
    
    public class VendorOrderDetailCredit: BaseEntityLong
    {
        public VendorOrderDetail VendorOrderDetail { get; set; }
        public long VendorOrderDetailId { get; set; }
        
        // public VendorOrder VendorOrder { get; set; }
        // public long VendorOrderId { get; set; }
        
        public decimal CreditQuantity { get; set; }
        public decimal CreditCost { get; set; }
        public decimal CreditAmount { get; set; }
        public CreditReasonType CreditReason { get; set; }
        public bool IsUnit { get; set; }
        
        #region Metadata

        public Product Product { get; set; }
        public long ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string ProductUpc { get; set; }
        
        public Vendor Vendor { get; set; }
        public long VendorId { get; set; }
        public string VendorName { get; set; }

        public int CasePack { get; set; }
        public decimal CaseCost { get; set; }
        
        #endregion
    }
} 