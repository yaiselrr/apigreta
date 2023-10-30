using System.Collections.Generic;
using System.Linq;
using Greta.BO.Api.Entities.Attributes;
using Greta.BO.Api.Entities.Enum;

namespace Greta.BO.Api.Entities.Lite
{
    [SqlTable("Fee")]
    public class LiteFee : BaseEntityLong
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
        [SqlFkTable("FamilyFee", "FeesId")]
        [SqlFkColumn("FamiliesId")]
        public virtual List<long> Families { get; set; }
        [SqlFkTable("CategoryFee", "FeesId")]
        [SqlFkColumn("CategoriesId")]
        public virtual List<long> Categories { get; set; }
        [SqlFkTable("FeeProduct", "FeesId")]
        [SqlFkColumn("ProductsId")]
        public virtual List<long> Products { get; set; }

        public static LiteFee Convert(Fee from)
        {
            return new()
            {
                Id = from.Id,
                State = from.State,
                UserCreatorId = from.UserCreatorId,
                CreatedAt = from.CreatedAt,
                UpdatedAt = from.UpdatedAt,
                Name = from.Name,
                Description = from.Description,
                Amount = from.Amount,
                Type = from.Type,
                IncludeInItemPrice = from.IncludeInItemPrice,
                ApplyDiscount = from.ApplyDiscount,
                ApplyFoodStamp = from.ApplyFoodStamp,
                ApplyAutomatically = from.ApplyAutomatically,
                ApplyToItemQty = from.ApplyToItemQty,
                ApplyTax = from.ApplyTax,
                RestrictToItems = from.RestrictToItems,
                Families = from.Families?.Select(x => x.Id).ToList(),
                Categories = from.Categories?.Select(x => x.Id).ToList(),
                Products = from.Products?.Select(x => x.Id).ToList(),
            };
        }
    }
}