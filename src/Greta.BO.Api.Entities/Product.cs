using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Greta.BO.Api.Entities.Enum;

namespace Greta.BO.Api.Entities
{
    [Table("Product")]
    public class Product : BaseEntityLong
    {
        public Product()
        {
            StoreProducts = new List<StoreProduct>();
            PriceBatchDetails = new List<PriceBatchDetail>();
            Discounts = new List<Discount>();
            KitProducts = new List<KitProduct>();
            VendorProducts = new List<VendorProduct>();
        }

        /// <summary>
        ///     UPC for a Scale Label product.
        ///     If the user is adding the UPC length of the UPC is 5 digits so we need to check Length on UPC entry on this type of
        ///     product. The length is a 5 digits number only 0-9.
        /// </summary>
        public string UPC { get; set; }
        public string UPC1 { get; set; }
        public string UPC2 { get; set; }
        public string UPC3 { get; set; }

        public string Name { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public string Name3 { get; set; }
        public long CategoryId { get; set; }
        public long DepartmentId { get; set; }
        public long? FamilyId { get; set; }
        public long? DefaulShelfTagId { get; set; }
        public ProductType ProductType { get; set; }
        public int? MinimumAge { get; set; }
        public bool PosVisible { get; set; }
        public bool ScaleVisible { get; set; }
        public bool AllowZeroStock { get; set; }
        public bool NoDiscountAllowed { get; set; }
        public bool PromptPriceAtPOS { get; set; }
        public bool SnapEBT { get; set; }
        public bool PrintShelfTag { get; set; }
        public bool NoPriceOnShelfTag { get; set; }
        public bool AddOnlineStore { get; set; }
        public bool Modifier { get; set; }
        public int LoyaltyPoints { get; set; }
        public bool DisplayStockOnPosButton { get; set; }
        public bool IsWeighted { get; set; }
        public decimal Tare1 { get; set; }

        public Department Department { get; set; }

        public Category Category { get; set; }
        public ScaleLabelType DefaulShelfTag { get; set; }

        public Family Family { get; set; }

        public List<VendorProduct> VendorProducts { get; set; }

        public virtual List<KitProduct> KitProducts { get; set; }

        public virtual List<Discount> Discounts { get; set; }

        public virtual List<PriceBatchDetail> PriceBatchDetails { get; set; }

        public List<StoreProduct> StoreProducts { get; set; }
        public virtual List<MixAndMatch> MixAndMatchs { get; set; }

        public virtual List<MixAndMatch> BuyMixAndMatchs { get; set; }
        public virtual List<Fee> Fees { get; set; }
        public virtual List<VendorOrderDetail> VendorOrderDetails { get; set; }

    }
}