using System.Collections.Generic;
using System.Linq;
using Greta.BO.Api.Entities.Attributes;
using Greta.BO.Api.Entities.Enum;

namespace Greta.BO.Api.Entities.Lite
{
    [SqlTable("Product")]
    public class LiteProduct : BaseEntityLong
    {
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

        // public virtual List<long> Discounts { get; set; }
        // public virtual List<long> Taxes { get; set; }
        // public virtual List<long> MixAndMatchs { get; set; }
        // public virtual List<long> BuyMixAndMatchs { get; set; }
        // public virtual List<long> Fees { get; set; }

        public static LiteProduct Convert(Product from, StoreProduct store)
        {
            return new()
            {
                Id = from.Id,
                State = from.State,
                UserCreatorId = from.UserCreatorId,
                CreatedAt = from.CreatedAt,
                UpdatedAt = from.UpdatedAt,
                UPC = from.UPC,
                UPC1 = from.UPC1,
                UPC2 = from.UPC2,
                UPC3 = from.UPC3,
                Name = from.Name,
                Name1 = from.Name1,
                Name2 = from.Name2,
                Name3 = from.Name3,
                CategoryId = from.CategoryId,
                DepartmentId = from.DepartmentId,
                FamilyId = from.FamilyId,
                DefaulShelfTagId = from.DefaulShelfTagId,
                ProductType = from.ProductType,
                MinimumAge = from.MinimumAge,
                PosVisible = from.PosVisible,
                ScaleVisible = from.ScaleVisible,
                AllowZeroStock = from.AllowZeroStock,
                NoDiscountAllowed = from.NoDiscountAllowed,
                PromptPriceAtPOS = from.PromptPriceAtPOS,
                SnapEBT = from.SnapEBT,
                PrintShelfTag = from.PrintShelfTag,
                NoPriceOnShelfTag = from.NoPriceOnShelfTag,
                AddOnlineStore = from.AddOnlineStore,
                Modifier = from.Modifier,
                LoyaltyPoints = from.LoyaltyPoints,
                DisplayStockOnPosButton = from.DisplayStockOnPosButton,
                // Discounts = from.Discounts?.Select(x => x.Id).ToList(),
                // Taxes = store.Taxs?.Select(x => x.Id).ToList(),
                // MixAndMatchs = from.MixAndMatchs,
                // BuyMixAndMatchs = from.BuyMixAndMatchs,
                // Fees = from.Fees?.Select(x => x.Id).ToList(),

                IsWeighted = from.IsWeighted,
                Tare1 = from.Tare1,
                    
                //Store product
                Price = store.Price,
                Price2 = store.Price2,
                Cost = store.Cost
            };
        }
        public bool IsWeighted { get; set; }
        public decimal Tare1 { get; set; }
        
        #region Price

        public decimal Price { get; set; }
        public decimal Price2 { get; set; }
        public decimal Cost { get; set; }
        public decimal GrossProfit { get; set; }
        
        #endregion Price
        
        public decimal QtyHand { get; set; }
        public decimal OrderTrigger { get; set; }
        public decimal OrderAmount { get; set; }
        
        
    }
}