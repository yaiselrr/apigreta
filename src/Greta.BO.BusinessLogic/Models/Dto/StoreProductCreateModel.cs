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
    public class StoreProductCreateModel : IDtoLong<string>, IMapFrom<StoreProduct>
    {
        public long StoreId { get; set; }

        public bool AllStores { get; set; }

        public long RegionId { get; set; }

        [Required] public long ProductId { get; set; }

        [Required] public decimal Price { get; set; }
        public decimal Price2 { get; set; }
        public decimal WebPrice { get; set; }
        public bool NoCategoryChange { get; set; }

        public decimal Cost { get; set; }
        public decimal GrossProfit { get; set; }
        public decimal GrossProfit2 { get; set; }
        public decimal WebGrossProfit { get; set; }
        
        public decimal TargetGrossProfit { get; set; }
        public List<long> TaxsIds { get; set; }

        public StoreModel Store { get; set; }
        public long Id { get; set; }
        public bool State { get; set; }
        public string UserCreatorId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<StoreProduct, StoreProductCreateModel>().ReverseMap()
                .ForMember(vm => vm.Taxs, m => m.MapFrom(u => u.TaxsIds.Select(x => new Tax {Id = x})));
        }
    }
}