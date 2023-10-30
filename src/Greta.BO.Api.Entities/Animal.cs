using System;
using System.Collections.Generic;
using Greta.Sdk.EFCore.Interfaces;

namespace Greta.BO.Api.Entities
{
    public class Animal : BaseEntityLong, IFullSyncronizable
    {
        public long StoreId { get; set; }
        public long? RancherId { get; set; }
        public string Tag { get; set; }
        public long? BreedId { get; set; }
        public List<Customer> Customers { get; set; }
        public DateTime? DateReceived { get; set; }
        public DateTime? DateSlaughtered { get; set; }
        public decimal? LiveWeight { get; set; }
        public decimal? RailWeight { get; set; }
        public decimal? SubPrimalWeight { get; set; }
        public decimal? CutWeight { get; set; }
        public Rancher Rancher { get; set; }
        public Breed Breed { get; set; }
        public Store Store { get; set; }
        
    
    }
}