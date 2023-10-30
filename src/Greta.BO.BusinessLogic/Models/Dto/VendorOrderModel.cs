using System;
using System.ComponentModel.DataAnnotations;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;
using Greta.Sdk.Core.Abstractions;

namespace Greta.BO.BusinessLogic.Models.Dto
{
    public class VendorOrderModel: IDtoLong<string>, IMapFrom<VendorOrder>
    {
        
        public VendorOrderVendorModel Vendor { get; set; }
        [Required]
        public long VendorId { get; set; }
        public VendorOrderUserModel User { get; set; }
        public long UserId { get; set; }
        [Required]
        public DateTime ReceivedDate { get; set; }
        public DateTime? OrderedDate { get; set; }
        public VendorOrderStoreModel Store { get; set; }
        [Required]
        public long StoreId { get; set; }
        public string AttachmentFilePath { get; set; }
        public string LastEmailId { get; set; }
        public int SendCount { get; set; }
        public string InvoiceNumber { get; set; }
        public decimal DeliveryCharge { get; set; }
        public VendorOrderStatus Status { get; set; }
        public bool IsDirectStoreDelivery { get; set; }
        
        public long Id { get; set; }

        public bool State { get; set; }
        public string UserCreatorId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class VendorOrderUserModel : IMapFrom<BOUser>
    {
        public long Id { get; set; }
        public string UserName { get; set; }
    }
    public class VendorOrderStoreModel : IMapFrom<Store>
    {
        public long Id { get; set; }
        public string Name { get; set; }
        
    }
    public class VendorOrderVendorModel : IMapFrom<Vendor>
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }
    
    public class VendorOrderReceiveModel : IMapFrom<BOUser>
    {
        public string InvoiceNumber { get; set; }
        public decimal DeliveryCharge { get; set; }
    }
}