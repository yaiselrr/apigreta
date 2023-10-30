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
    public class StoreProductModel : IDtoLong<string>, IMapFrom<StoreProduct>
    {
        [Required] public long StoreId { get; set; }

        [Required] public long ProductId { get; set; }
        public ProductModel Product { get; set; }

        public Store Store { get; set; }

        [Required] public decimal Price { get; set; }
        public decimal Price2 { get; set; }
        public decimal WebPrice { get; set; }
        public bool NoCategoryChange { get; set; }
        [Required] public decimal Cost { get; set; }

        [Required] public decimal GrossProfit { get; set; }
        public decimal GrossProfit2 { get; set; }
        public decimal WebGrossProfit { get; set; }
        public decimal TargetGrossProfit { get; set; }
        public List<long> TaxsIds { get; set; }
        public List<TaxModel> Taxs { get; set; }
        public long Id { get; set; }
        public bool State { get; set; }
        public string UserCreatorId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        
        #region BreakPack
        public long? ParentId { get; set; }
        public StoreProductModel Parent { get; set; }
        public long? ChildId { get; set; }
        public StoreProductModel Child { get; set; }
        public decimal SplitCount { get; set; }
        #endregion BreakPack

        public void Mapping(Profile profile)
        {
            profile.CreateMap<StoreProduct, StoreProductModel>().ReverseMap()
                .ForMember(vm => vm.Taxs, m => m.MapFrom(u => u.TaxsIds.Select(x => new Tax {Id = x})));
        }
    }
}