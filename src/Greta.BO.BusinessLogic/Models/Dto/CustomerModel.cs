using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AutoMapper;
using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;
using Greta.BO.BusinessLogic.Models.Dto.MixAndMatchDto;
using Greta.Sdk.Core.Abstractions;

namespace Greta.BO.BusinessLogic.Models.Dto
{
    public class CustomerModel : IDtoLong<string>, IMapFrom<Customer>
    {
        public string FullName { get; set; }

        [Required]
        [StringLength(40, ErrorMessage = "The {0} field not is valid")]
        public string LastName { get; set; }

        [Required]
        [StringLength(40, ErrorMessage = "The {0} field not is valid")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(250, ErrorMessage = "The {0} field not is valid")]
        public string Phone { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} field not is valid")]
        public string Email { get; set; }
        
        public bool TaxExcept { get; set; }
        
        public bool UsePrice2 { get; set; }
        
        [StringLength(64, ErrorMessage = "The {0} field not is valid")]
        public string TaxID { get; set; }

        //[Required] 
        public string Address1 { get; set; }

        public string Address2 { get; set; }
        public string CityName { get; set; }
        public string ProvinceName { get; set; }
        public string CountryName { get; set; }

        //[Required] public long CityId { get; set; }

        [Required] public long ProvinceId { get; set; }

        [Required] public long CountryId { get; set; }

        //[Required] 
        public string Zip { get; set; }
        
        public int StoreCredit { get; set; }
        
        public DateTime LastBuy { get; set; }
       
        public List<long> MixAndMatchIds { get; set; }
        public List<MixAndMatchModel> MixAndMatches { get; set; }
        
        public List<long> DiscountIds { get; set; }
        public List<DiscountModel> Discounts { get; set; }

        public long Id { get; set; }

        public bool State { get; set; }
        public string UserCreatorId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Customer, CustomerModel>().ReverseMap()
                .ForMember(vm => vm.MixAndMatches, m => m.MapFrom(u => u.MixAndMatchIds.Select(x => new MixAndMatch() {Id = x})))
                .ForMember(vm => vm.Discounts, m => m.MapFrom(u => u.DiscountIds.Select(x => new Discount() {Id = x})));
        }
    }
}