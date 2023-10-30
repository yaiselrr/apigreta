using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AutoMapper;
using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;
using Greta.Sdk.Core.Abstractions;

namespace Greta.BO.BusinessLogic.Models.Dto
{
    public class AnimalModel : IDtoLong<string>, IMapFrom<Animal>
    {
        [Required] public long? StoreId { get; set; }
        [Required] public long? RancherId { get; set; }
        [Required] public string Tag { get; set; }
        [Required] public long? BreedId { get; set; }
        public List<long> CustomersIds { get; set; }
        public DateTime? DateReceived { get; set; }
        public DateTime? DateSlaughtered { get; set; }
        public decimal? LiveWeight { get; set; }
        public decimal? RailWeight { get; set; }
        public decimal? SubPrimalWeight { get; set; }
        public decimal? CutWeight { get; set; }

        public Rancher Rancher { get; set; }
        public Breed Breed { get; set; }

        public bool State { get; set; }
        public string UserCreatorId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public long Id { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Animal, AnimalModel>()
                .ForMember(vm => vm.CustomersIds,
                    m => 
                        m.MapFrom(u => u.Customers == null ? new List<long>() : u.Customers.Select(x => x.Id).ToList() ))
                .ReverseMap()
                .ForMember(vm => vm.Customers,
                    m => m.MapFrom(u => u.CustomersIds.Select(x => new Customer() { Id = x })));
        }
    }
}