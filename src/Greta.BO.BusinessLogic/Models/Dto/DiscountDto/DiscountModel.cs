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
    public class DiscountModel : IDtoLong<string>, IMapFrom<Discount>
    {
        [Required]
        [StringLength(64, ErrorMessage = "The {0} field not is valid")]
        public string Name { get; set; }

        [Required] public DiscountType Type { get; set; }

        [Required] public decimal Value { get; set; }
        
        public bool NotAllowAnyOtherDiscount { get; set; }
        public bool ApplyToTotalSale { get; set; }
        public bool PromptForPrice { get; set; }
        public bool ApplyToProduct { get; set; }
        public bool ApplyAutomatically { get; set; }
        public bool ApplyToCustomerOnly { get; set; }
        public bool ActiveOnPeriod { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? Monday { get; set; }
        public bool? Tuesday { get; set; }
        public bool? Wednesday { get; set; }
        public bool? Thursday { get; set; }
        public bool? Friday { get; set; }
        public bool? Saturday { get; set; }
        public bool? Sunday { get; set; }

        public long? DepartmentId { get; set; }
        public long? CategoryId { get; set; }

        public DepartmentModel Department { get; set; }
        public CategoryModel Category { get; set; }

        public virtual List<long> ProductIds { get; set; }
        public virtual List<ProductModel> Products { get; set; }


        public long Id { get; set; }
        public bool State { get; set; }
        public string UserCreatorId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }


        public void Mapping(Profile profile)
        {
            profile.CreateMap<Discount, DiscountModel>().ReverseMap()
                .ForMember(vm => vm.Products, m => m.MapFrom(u => u.ProductIds.Select(x => new Product {Id = x})));
        }
    }
}