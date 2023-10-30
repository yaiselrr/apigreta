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
    public class CutListTemplateModel : IDtoLong<string>, IMapFrom<CutListTemplate>
    {
        [Required]
        [StringLength(30, ErrorMessage = "The {0} field not is valid")]
        public string Name { get; set; }

        public long Id { get; set; }

        public bool State { get; set; }
        public string UserCreatorId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public List<long> ScaleProductIds { get; set; }
        public List<ScaleProductModel> ScaleProducts { get; set; }      

        public void Mapping(Profile profile)
        {           
            profile.CreateMap<CutListTemplate, CutListTemplateModel>().ReverseMap()
               .ForMember(vm => vm.ScaleProducts, m => m.MapFrom(u => u.ScaleProductIds.Select(x => new ScaleProduct { Id = x }).ToList()));
        }
    }
}