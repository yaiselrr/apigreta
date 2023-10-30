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
    public class OnlineStoreModel : IDtoLong<string>, IMapFrom<OnlineStore>
    {
        [Required]
        [StringLength(64, ErrorMessage = "The {0} field not is valid")]
        public string Name { get; set; }
        public string NameWebsite { get; set; }
        public LocationServerType LocationServerType { get; set; }
        public bool IsActiveWebSite { get; set; }
        public bool IsStockUpdated { get; set; }
        public bool IsAllowStorePickup { get; set; }     
        public bool IsAssociated { get; set; }     
        public bool Isdeleted { get; set; } 
        public string Instance { get; set; }
        public string Url { get; set; }
        public string CustomDns { get; set; }
        [Required] public long StoreId { get; set; }
        public List<long> DepartmentsIds { get; set; }
        public List<DepartmentModel> Departments { get; set; }

        public long Id { get; set; }
        public bool State { get; set; }
        public string UserCreatorId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<OnlineStore, OnlineStoreModel>().ReverseMap()
                .ForMember(vm => vm.Departments, m => m.MapFrom(u => u.DepartmentsIds.Select(x => new Department() {Id = x})));
        }
    }
}