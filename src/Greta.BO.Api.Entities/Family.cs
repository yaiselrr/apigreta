using System.Collections.Generic;
using Greta.BO.Api.Entities.Interfaces;
using Greta.Sdk.EFCore.Interfaces;

namespace Greta.BO.Api.Entities
{
    public class Family : BaseEntityLong, IFullSyncronizable, INameUniqueEntity
    {
        public string Name { get; set; }


        //public List<Product> Products { get; set; }

        public virtual List<Discount> Discounts { get; set; }
        public virtual List<Product> Products { get; set; }

        public virtual List<MixAndMatch> MixAndMatchs { get; set; }

        public virtual List<Fee> Fees { get; set; }

        public virtual List<PriceBatchDetail> PriceBatchDetails { get; set; }
    }
}