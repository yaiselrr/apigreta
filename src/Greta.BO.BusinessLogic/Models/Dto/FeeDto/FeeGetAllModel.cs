using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AutoMapper;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;
using Greta.Sdk.Core.Abstractions;

namespace Greta.BO.BusinessLogic.Models.Dto
{
    public class FeeGetAllModel : IDtoLong<string>, IMapFrom<Fee>
    {
        [Required]
        [StringLength(30, ErrorMessage = "The {0} field not is valid")]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required] public decimal Amount { get; set; }

        [Required] public DiscountType Type { get; set; }

        public bool IncludeInItemPrice { get; set; }
        public bool ApplyDiscount { get; set; }
        public bool ApplyFoodStamp { get; set; }
        public bool ApplyAutomatically { get; set; }
        public bool ApplyToItemQty { get; set; }
        public bool ApplyTax { get; set; }
        public bool RestrictToItems { get; set; }


        public List<long> FamilyIds { get; set; }
        public List<long> CategoryIds { get; set; }
        public List<long> ProductIds { get; set; }

        public List<FamilyModel> Families { get; set; }
        public List<CategoryForFeeModel> Categories { get; set; }
        public List<ProductForFeeModel> Products { get; set; }


        public long Id { get; set; }
        public bool State { get; set; }
        public string UserCreatorId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Fee, FeeGetAllModel>().ReverseMap()
                .ForMember(vm => vm.Products,
                    m => m.MapFrom(u => u.ProductIds.Select(x => new Product {Id = x})))
                .ForMember(vm => vm.Categories,
                    m => m.MapFrom(u => u.CategoryIds.Select(x => new Category {Id = x})))
                .ForMember(vm => vm.Families,
                    m => m.MapFrom(u => u.FamilyIds.Select(x => new Family {Id = x})));
        }
    }
}