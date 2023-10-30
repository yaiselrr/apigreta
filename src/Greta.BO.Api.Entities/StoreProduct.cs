using System.Collections.Generic;

namespace Greta.BO.Api.Entities
{
    public class StoreProduct : BaseEntityLong
    {
        public long ProductId { get; set; }

        public long StoreId { get; set; }


        public Product Product { get; set; }
        public Store Store { get; set; }

        public virtual List<Tax> Taxs { get; set; }
        
        public virtual List<OnlineProduct> OnlineProducts { get; set; }

        #region BreakPack
        public long? ParentId { get; set; }
        public StoreProduct Parent { get; set; }
        public long? ChildId { get; set; }
        public StoreProduct Child { get; set; }
        public decimal SplitCount { get; set; }
        #endregion BreakPack
        
        #region Price

        /// <summary>
        /// This price store the backup price when one ad batch is apply to this product for return to the old price
        /// </summary>
        public decimal? BatchOldPrice { get; set; }
        /// <summary>
        /// This price store the backup price when the price is changed by a targetGrossProfit from Category
        /// </summary>
        public decimal? TargetOldPrice { get; set; }
        public decimal Price { get; set; }
        public decimal Price2 { get; set; }
        public decimal WebPrice { get; set; }

        public decimal Cost { get; set; }
        
        public bool NoCategoryChange { get; set; }

        /// <summary>
        ///     Percent 0.00   format  7.25%
        ///     Gross Profit if Cost = 0.00 Then Gross Profit = 0.00 Else Gross Profit = Retail - Cost / Retail
        ///     “No negative number allowed in Retail or Cost”. Then mark the record as CHANGED
        /// </summary>
        public decimal GrossProfit { get; set; }
        public decimal GrossProfit2 { get; set; }
        public decimal WebGrossProfit { get; set; }
        public decimal TargetGrossProfit { get; set; }

        #endregion Price

        // public virtual List<Discount> Discounts { get; set; }
        
        
        #region Inventory
        
        /// <summary>
        ///     Bin Location tells where the product is displayed in the store.
        ///     <example>
        ///         isle 5 section 6 shelf 3
        ///     </example>
        /// </summary>
        public long? BinLocationId { get; set; }
        public BinLocation BinLocation { get; set; }

        /// <summary>
        ///     Qty on Hand
        /// </summary>
        public decimal QtyHand { get; set; }

        /// <summary>
        ///     Not defined yet
        /// </summary>
        public decimal OrderTrigger { get; set; }

        /// <summary>
        ///     Not defined yet
        /// </summary>
        public decimal OrderAmount { get; set; }

        #endregion Inventory
    }
}