using System.Collections.Generic;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.Api.Entities.Interfaces;
using Greta.Sdk.EFCore.Interfaces;

namespace Greta.BO.Api.Entities
{
    public class Fee : BaseEntityLong, IFullSyncronizable, INameUniqueEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public DiscountType Type { get; set; }
        public bool IncludeInItemPrice { get; set; }
        public bool ApplyDiscount { get; set; }
        public bool ApplyFoodStamp { get; set; }
        public bool ApplyAutomatically { get; set; }
        public bool ApplyToItemQty { get; set; }
        public bool ApplyTax { get; set; }
        public bool RestrictToItems { get; set; }

        public virtual List<Family> Families { get; set; }
        public virtual List<Category> Categories { get; set; }
        public virtual List<Product> Products { get; set; }
    }
}