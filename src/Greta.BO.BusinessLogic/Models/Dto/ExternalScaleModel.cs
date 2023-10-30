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
    public class ExternalScaleModel : IDtoLong<string>, IMapFrom<ExternalScale>
    {
        [Required]
        [StringLength(64, ErrorMessage = "The {0} field not is valid")]
        public string Ip { get; set; }
        
        [StringLength(64, ErrorMessage = "The {0} field not is valid")]
        public string Port { get; set; }

        [Required] 
        public BoExternalScaleType ExternalScaleType { get; set; }
        
        public List<long> DepartmentsIds { get; set; }
        public List<DepartmentModel> Departments { get; set; }
        
        //public long ScaleBrandId { get; set; }

        //public ScaleBrandModel ScaleBrand { get; set; }

        public long StoreId { get; set; }

        public StoreModel Store { get; set; }
        
        public long? SyncDeviceId { get; set; }
        public DeviceModel SyncDevice { get; set; }

        public bool State { get; set; }
        public string UserCreatorId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public long Id { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<ExternalScale, ExternalScaleModel>().ReverseMap()
                .ForMember(vm => vm.Departments, m => m.MapFrom(u => u.DepartmentsIds.Select(x => new Department() {Id = x})));
        }
    }
}