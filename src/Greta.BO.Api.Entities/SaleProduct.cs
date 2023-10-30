using Greta.BO.Api.Entities.Enum;

namespace Greta.BO.Api.Entities
{
    public class SaleProduct : BaseEntityLong
    {
        public SaleProductType SaleProductType { get; set; } 
        public long SaleId { get; set; }
       
        public long ProductId { get; set; }
        
        public long DepartmentId { get; set; }

        public string Subtitle { get; set; }
        
        public bool SubtitleVisible { get; set; }
        public decimal DiscountValue { get; set; }

        #region StandarProduct

        public string UPC { get; set; }
        public string Name { get; set; }
        public string UsedUPC { get; set; }
        public string UsedName { get; set; }
        
        //public long CategoryId { get; set; }
        //public long DepartmentId { get; set; }
        //public long? FamilyId { get; set; }
        //public long? DefaulShelfTagId { get; set; }
        public ProductType ProductType { get; set; }

        public decimal Price { get; set; }
        public decimal Cost { get; set; }
        public decimal GrossProfit { get; set; }

        #endregion

        #region OtherFields for products

        public int? PLUNumber { get; set; }
        //public long? ScaleCategoryId { get; set; }
        //public PluType PLUType { get; set; }

        //public int? ByCount { get; set; }

        //public int? ShelfLife { get; set; }

        //public int? ProductLife { get; set; }

        //public decimal? PackageWeight { get; set; }

        public decimal? NetWeigth { get; set; }

        #endregion

        #region Sales Fields

        public decimal MixMatchDiscount { get; set; } = -1;
        public MixAndMatchType MixAndMatchType { get; set; }
        public string MixAndMatchName { get; set; }
        public decimal MixAndMatchGlobalDiscount { get; set; }
        public SaleProductDiscountApplied SaleProductDiscountApplied { get; set; }

        public long SaleDiscountId { get; set; }
        //public decimal DiscountValue { get; set; }

        public decimal TaxValue { get; set; }
        public decimal QTY { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal CleanTotalPrice { get; set; }
        

        //public List<SaleTax> Taxs { get; set; }
        public int LoyaltyPoints { get; set; }
        public Sale Sale { get; set; }
        #endregion

        #region Discount zone

        public string DiscountName { get; set; }
        public DiscountType DiscountType { get; set; }

        public decimal DiscountAmount { get; set; }

        public bool DiscountApplyToProduct { get; set; }
        public bool DiscountApplyAutomatically { get; set; }
        public bool DiscountApplyToCustomerOnly { get; set; }
        #endregion

    }
}
