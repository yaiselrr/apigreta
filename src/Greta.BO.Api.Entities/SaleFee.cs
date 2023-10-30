using Greta.BO.Api.Entities.Enum;

namespace Greta.BO.Api.Entities
{
    public class SaleFee : BaseEntityLong
    {
        public long? SaleId { get; set; }
        public Sale Sale { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public decimal Value { get; set; }
        public DiscountType Type { get; set; }
        public bool IncludeInItemPrice { get; set; }
        public bool ApplyDiscount { get; set; }
        public bool ApplyFoodStamp { get; set; }
        public bool ApplyAutomatically { get; set; }
        public bool ApplyToItemQty { get; set; }
        public bool ApplyTax { get; set; }
        public bool RestrictToItems { get; set; }

     
    }
}
