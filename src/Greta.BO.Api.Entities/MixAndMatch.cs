using System;
using System.Collections.Generic;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.Api.Entities.Interfaces;
using Greta.Sdk.EFCore.Interfaces;

namespace Greta.BO.Api.Entities
{
    public class MixAndMatch : BaseEntityLong, IFullSyncronizable, INameUniqueEntity
    {
        public string Name { get; set; }

        /// <summary>
        ///     Fix or percent amount
        /// </summary>
        /// <value></value>
        public decimal Amount { get; set; }

        /// <summary>
        ///     QTY for activate Match
        /// </summary>
        /// <value></value>
        public int QTY { get; set; }

        //Type
        public MixAndMatchType MixAndMatchType { get; set; }

        /// <summary>
        ///     Only Use Dates if this is active
        /// </summary>
        /// <value></value>
        public bool ActivePeriod { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public long? ProductBuyId { get; set; }
        
        public bool NotAllowAnyOtherDiscount { get; set; }
        public bool ApplyToCustomerOnly { get; set; }

        /// <summary>
        ///     Product to buy for activate the BuyOneGetFree
        /// </summary>
        public Product ProductBuy { get; set; }

        /// <summary>
        ///     Families to apply the MIx and Match
        /// </summary>
        public List<Family> Families { get; set; }

        /// <summary>
        ///     Not family product to apply the mix and match
        /// </summary>
        public List<Product> Products { get; set; }
        
        /// <summary>
        /// Customers to apply the mix and match
        /// </summary>
        public virtual List<Customer> Customers { get; set; }
    }
}