using System;
using System.Collections.Generic;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;
using Greta.Sdk.Core.Abstractions;

namespace Greta.BO.BusinessLogic.Models.Dto
{
    public class InventoryResponseModel : IDtoLong<string>, IMapFrom<StoreProduct>
    {
        public long ProductId { get; set; }
        public ProductInventoryModel Product { get; set; }
        public long BinLocationId { get; set; }
        public BinLocationInventoryModel BinLocation { get; set; }
        public decimal QtyHand { get; set; }
        public decimal OrderTrigger { get; set; }
        public decimal OrderAmount { get; set; }
        
        
        public long Id { get; set; }
        public bool State { get; set; }
        public string UserCreatorId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
    
    public class BinLocationResponseModel : IDtoLong<string>, IMapFrom<StoreProduct>
    {
        public ProductInventoryModel Product { get; set; }
        public long Id { get; set; }
        public bool State { get; set; }
        public string UserCreatorId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
    

    public class ProductInventoryModel: IMapFrom<Product>, IDtoLong<string>
    {
        public long Id { get; set; }
        public string UPC { get; set; }
        public string Name { get; set; }

        public bool State { get; set; }
        public string UserCreatorId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public List<VendorProductForInventory> VendorProducts { get; set; }
    }

    public class VendorProductForInventory : IMapFrom<VendorProduct>
    {
        public string ProductCode { get; set; }
        public bool IsPrimary { get; set; }
        public int CasePack { get; set; }
        public decimal CaseCost { get; set; }
        public decimal UnitCost { get; set; }
        public VendorProductType OrderByCase { get; set; }
        public DateTime? LastOrderDate { get; set; }

        public long VendorId { get; set; }
        public long ProductId { get; set; }
    }

    public class BinLocationInventoryModel: IMapFrom<BinLocation>
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }
}