using System;
using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.Sdk.Core.Abstractions;

namespace Greta.BO.BusinessLogic.Models
{
    public class StoreProductSetParentResponse: IDtoLong<string>, IMapFrom<StoreProduct>
    {
        
        
        public ProductModel Product { get; set; }
        
        public long Id { get; set; }
        public bool State { get; set; }
        public string UserCreatorId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        
        
        #region BreakPack
        public long? ParentId { get; set; }
        public StoreProductSetParentResponse Parent { get; set; }
        public long? ChildId { get; set; }
        public StoreProductSetParentResponse Child { get; set; }
        public decimal SplitCount { get; set; }
        #endregion BreakPack
    }
}