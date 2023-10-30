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
    public class CutListModel : IDtoLong<string>, IMapFrom<CutList>
    {
        [Required]
        public long AnimalId { get; set; }
        [Required]
        public long CustomerId { get; set; }
        
        public AnimalModel Animal { get; set; }
        public CustomerModel Customer { get; set; }
        
        public CutListType CutListType { get; set; }
        public string SpecialInstruction { get; set; }
        
        public List<long> CutListDetailsIds { get; set; } 
        public List<CutListDetailModel> CutListDetails { get; set; }

        public long? CutListTemplateId { get; set; }
        public CutListTemplateModel CutListTemplateModel { get; set; }

        public long Id { get; set; }

        public bool State { get; set; }
        public string UserCreatorId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }        

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CutList, CutListModel>().ReverseMap()
                .ForMember(vm => vm.CutListDetails, m => m.MapFrom(u => u.CutListDetailsIds.Select(x => new CutListDetail() {Id = x})));
        }
    }
}