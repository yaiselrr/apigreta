using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AutoMapper;
using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;
using Greta.Sdk.Core.Abstractions;

namespace Greta.BO.BusinessLogic.Models.Dto
{
    public class StoreProductMinimalModel : IDtoLong<string>, IMapFrom<StoreProduct>
    {
        public ProductInventoryModel Product { get; set; }
        public decimal OrderAmount { get; set; }
        public decimal QtyHand { get; set; }
        public int CasePack { get; set; }
        
        public string PackSize { get; set; }
        public decimal CaseCost { get; set; }
        public string ProductCode { get; set; }
        
        public decimal Cost { get; set; }
        public decimal Price { get; set; }
        public decimal GrossProfit { get; set; }
        
        public bool NoCategoryChange { get; set; }
        public decimal TargetGrossProfit { get; set; }
        
        public long Id { get; set; }
        public bool State { get; set; }
        public string UserCreatorId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}