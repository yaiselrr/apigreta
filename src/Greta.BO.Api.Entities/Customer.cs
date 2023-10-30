using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Greta.Sdk.EFCore.Interfaces;

namespace Greta.BO.Api.Entities
{
    public class Customer : BaseLocationEntityLong, IFullSyncronizable
    {
        public string LastName { get; set; }

        public string FirstName { get; set; }

        public bool UsePrice2 { get; set; }
        public string Phone { get; set; }

        public string Email { get; set; }

        public bool TaxExcept { get; set; }
        public bool MarketingAllowed { get; set; }
        
        public string TaxID { get; set; }

        public int StoreCredit { get; set; }
        
        [NotMapped]
        public decimal DiscountMaximum { get; set; }
        [NotMapped]
        public decimal DiscountValue { get; set; }
        
        public DateTime LastBuy { get; set; }


        /// <summary>
        /// Mix and Matchs to apply to the customer
        /// </summary>
        public List<MixAndMatch> MixAndMatches { get; set; }
        
        /// <summary>
        /// Discounts to apply to the customer
        /// </summary>
        public List<Discount> Discounts { get; set; }
        public List<Animal> Animals { get; set; }
        
    }
}