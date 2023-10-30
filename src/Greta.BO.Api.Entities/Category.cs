using System.Collections.Generic;
using Greta.BO.Api.Entities.Interfaces;
using Greta.Sdk.EFCore.Interfaces;

namespace Greta.BO.Api.Entities
{
    public class Category : BaseEntityLong, IFullSyncronizable, INameUniqueEntity
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long DepartmentId { get; set; }

        public bool VisibleOnPos { get; set; }

        public long? DefaulShelfTagId { get; set; }

        public bool PromptPriceAtPOS { get; set; }

        public bool SnapEBT { get; set; }

        public bool PrintShelfTag { get; set; }

        public bool NoPriceOnShelfTag { get; set; }

        public bool AllowZeroStock { get; set; }

        public int? MinimumAge { get; set; }
        public bool NoDiscountAllowed { get; set; }
        public bool AddOnlineStore { get; set; }
        public bool Modifier { get; set; }

        public bool DisplayStockOnPosButton { get; set; }
        public string BackgroundColor { get; set; }
        public string ForegroundColor { get; set; }

        public decimal TargetGrossProfit { get; set; }
        
        public bool IsLiquorCategory { get; set; }
        public List<PriceBatchDetail> PriceBatchDetails { get; set; }
        public virtual List<Tax> Taxs { get; set; }
        public ScaleLabelType DefaulShelfTag { get; set; }
        public Department Department { get; set; }
        public virtual List<Discount> Discounts { get; set; }
        public virtual List<Product> Products { get; set; }
        public virtual List<Fee> Fees { get; set; }
        
        public virtual List<OnlineCategory> OnlineCategories { get; set; }
    }
}