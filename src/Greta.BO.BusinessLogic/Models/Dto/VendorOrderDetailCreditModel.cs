using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;
using Greta.Sdk.Core.Abstractions;

namespace Greta.BO.BusinessLogic.Models.Dto
{
    public class VendorDetailCreditListModel
    {
        public List<VendorOrderDetailCreditModel> Elements { get; set; }
        
        public long VendorOrderDetail { get; set; }
    }
    public class VendorOrderDetailCreditModel: IDtoLong<string>, IMapFrom<VendorOrderDetailCredit>
    {
        public VendorOrderDetailModel VendorOrderDetail { get; set; }
        [Required]
        public long VendorOrderDetailId { get; set; }
        
        [Required]
        public decimal CreditQuantity { get; set; }
        [Required]
        public decimal CreditCost { get; set; }
        public decimal CreditAmount { get; set; }
        [Required]
        public CreditReasonType CreditReason { get; set; }
        public bool IsUnit { get; set; }
        
        
        public Product Product { get; set; }
        [Required]
        public long ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string ProductUpc { get; set; }
        
        public Vendor Vendor { get; set; }
        [Required]
        public long VendorId { get; set; }
        
        public int CasePack { get; set; }
        public decimal CaseCost { get; set; }

        public long Id { get; set; }

        public bool State { get; set; }
        public string UserCreatorId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}