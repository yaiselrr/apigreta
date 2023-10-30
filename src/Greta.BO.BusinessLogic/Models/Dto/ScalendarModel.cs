using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;
using Greta.Sdk.Core.Abstractions;

namespace Greta.BO.BusinessLogic.Models.Dto
{
    public class ScalendarModel : IDtoLong<string>, IMapFrom<Scalendar>
    {
        public string Day { get; set; }

        public List<long> BreedIds { get; set; }

        public virtual List<Breed> Breeds { get; set; }

        public long Id { get; set; }
        public bool State { get; set; }
        public string UserCreatorId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Scalendar, ScalendarModel>().ReverseMap()
                .ForMember(vm => vm.Breeds,
                    m => m.MapFrom(u => u.BreedIds.Select(x => new BreedModel {Id = x})));
        }
    }
}