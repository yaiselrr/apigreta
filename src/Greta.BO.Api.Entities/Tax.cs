using System.Collections.Generic;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.Api.Entities.Interfaces;
using Greta.Sdk.EFCore.Interfaces;

namespace Greta.BO.Api.Entities
{
    public class Tax : BaseEntityLong, INameUniqueEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public TaxType Type { get; set; }
        public double Value { get; set; }
        public double? SpecialValue { get; set; }

        public virtual List<Store> Stores { get; set; }

        public virtual List<StoreProduct> StoreProducts { get; set; }

        public virtual List<Category> Categories { get; set; }
    }
}