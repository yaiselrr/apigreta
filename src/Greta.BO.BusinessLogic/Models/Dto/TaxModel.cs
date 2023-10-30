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
    public class TaxModel : IDtoLong<string>, IMapFrom<Tax>
    {
        [Required]
        [StringLength(64, ErrorMessage = "The {0} field not is valid")]
        public string Name { get; set; }

        [Required]
        [StringLength(254, ErrorMessage = "The {0} field not is valid")]
        public string Description { get; set; }

        [Required] public TaxType Type { get; set; }

        [Required] public double Value { get; set; }
        public double? SpecialValue { get; set; }

        public List<long> StoresId { get; set; }

        public List<StoreModel> Stores { get; set; }

        public bool State { get; set; }
        public string UserCreatorId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public long Id { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Tax, TaxModel>().ReverseMap()
                .ForMember(vm => vm.Stores, m => m.MapFrom(u => u.StoresId.Select(x => new Store {Id = x})));
        }
    }
}