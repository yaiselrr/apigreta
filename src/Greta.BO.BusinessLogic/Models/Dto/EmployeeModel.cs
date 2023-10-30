using System;
using System.ComponentModel.DataAnnotations;
using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;
using Greta.Sdk.Core.Abstractions;
using System.Collections.Generic;
using AutoMapper;
using System.Linq;

namespace Greta.BO.BusinessLogic.Models.Dto
{
    public class EmployeeModel : IDtoLong<string>, IMapFrom<Employee>
    {
        [Required]
        [StringLength(40, ErrorMessage = "The {0} field not is valid (length)")]
        public string LastName { get; set; }

        [Required]
        [StringLength(40, ErrorMessage = "The {0} field not is valid (length)")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "The {0} field not is valid (length)")]
        public string Phone { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} field not is valid (length)")]
        public string Email { get; set; }

        [StringLength(6, ErrorMessage = "The {0} field not is valid (length)")]
        public string Password { get; set; }

        [Required] public string Address1 { get; set; }

        public string Address2 { get; set; }
        public string CityName { get; set; }
        public string ProvinceName { get; set; }
        public string CountryName { get; set; }

        //[Required] public long CityId { get; set; }

        [Required] public long ProvinceId { get; set; }

        [Required] public long CountryId { get; set; }

        [Required] public string Zip { get; set; }

        public long Id { get; set; }
        public bool State { get; set; }
        public string UserCreatorId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public List<long> StoresId { get; set; }

        public List<StoreModel> Stores { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Employee, EmployeeModel>().ReverseMap()
                .ForMember(vm => vm.Stores, m => m.MapFrom(u => u.StoresId.Select(x => new StoreModel { Id = x })));
        }
    }
}