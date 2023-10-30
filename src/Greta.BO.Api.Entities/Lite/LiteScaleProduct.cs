using System.Collections.Generic;
using System.Linq;
using Greta.BO.Api.Entities.Attributes;
using Greta.BO.Api.Entities.Enum;

namespace Greta.BO.Api.Entities.Lite
{
    [SqlTable("ScaleProduct")]
    public class LiteScaleProduct : LiteProduct
    {
        public int PLUNumber { get; set; }
        public long ScaleCategoryId { get; set; }
        // public LiteScaleCategory ScaleCategory { get; set; }
        public PluType PLUType { get; set; }
        public string Description1 { get; set; }
        public string Description2 { get; set; }
        public string Description3 { get; set; }

        /// <summary>
        ///     By Count is the number of units in a package. Like I have a package of Dinner rolls there are 6 in the package. So
        ///     By count would be 6
        /// </summary>
        public int ByCount { get; set; }

        public int ShelfLife { get; set; }

        public int ProductLife { get; set; }

        public decimal PackageWeight { get; set; }

        #region Scale Label type

        // public List<long> ScaleLabelDefinitions { get; set; }

        #endregion Scale Label type


        // [SqlFkTable("ScaleHomeFavScaleProduct", "ScaleProductsId")]
        // [SqlFkColumn("ScaleHomeFavsId")]
        // public virtual List<long> ScaleHomeFavs { get; set; }

        public static LiteScaleProduct Convert1(ScaleProduct from, StoreProduct store)
        {
            return new()
            {
                Id = from.Id,
                State = from.State,
                UserCreatorId = from.UserCreatorId,
                CreatedAt = from.CreatedAt,
                UpdatedAt = from.UpdatedAt,
                UPC = from.UPC,
                Name = from.Name,
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
                // Discounts = from.Discounts.Select(x => x.Id).ToList(),
                // Taxes = store.Taxs?.Select(x => x.Id).ToList(),
                // MixAndMatchs = from.MixAndMatchs,
                // BuyMixAndMatchs = from.BuyMixAndMatchs,
                // Fees = from.Fees?.Select(x => x.Id).ToList(),

                PLUNumber = from.PLUNumber,
                ScaleCategoryId = from.ScaleCategoryId,
                // ScaleCategory = from.ScaleCategory,
                PLUType = from.PLUType,
                Description1 = from.Description1,
                Description2 = from.Description2,
                Description3 = from.Description3,
                ByCount = from.ByCount,
                ShelfLife = from.ShelfLife,
                ProductLife = from.ProductLife,
                PackageWeight = from.PackageWeight,
                Text1 = from.Text1,
                Text2 = from.Text2,
                Text3 = from.Text3,
                Text4 = from.Text4,
                Tare1 = from.Tare1,
                Tare2 = from.Tare2,
                ForceTare = from.ForceTare,
                TareIsPercent = from.TareIsPercent,
                ServingPerContainer = from.ServingPerContainer,
                ServingSize = from.ServingSize,
                AmountPerServingCalories = from.AmountPerServingCalories,
                TotalFatGrams = from.TotalFatGrams,
                TotalFat = from.TotalFat,
                SaturedFatGrams = from.SaturedFatGrams,
                SaturedFat = from.SaturedFat,
                CholesterolMGrams = from.CholesterolMGrams,
                Cholesterol = from.Cholesterol,
                SodiumMGrams = from.SodiumMGrams,
                Sodium = from.Sodium,
                TotalCarbohydrateGrams = from.TotalCarbohydrateGrams,
                TotalCarbohydrate = from.TotalCarbohydrate,
                DietaryFiberGrams = from.DietaryFiberGrams,
                DietaryFiber = from.DietaryFiber,
                TotalSugarGrams = from.TotalSugarGrams,
                AddedSugarGrams = from.AddedSugarGrams,
                AddedSugar = from.AddedSugar,
                ProteinGrams = from.ProteinGrams,
                VitDMGrams = from.VitDMGrams,
                VitD = from.VitD,
                CalciumMGrams = from.CalciumMGrams,
                Calcium = from.Calcium,
                IronMGrams = from.IronMGrams,
                Iron = from.Iron,
                PotasMGrams = from.PotasMGrams,
                Potas = from.Potas,

                //ScaleLabelDefinitions = from.ScaleLabelDefinitions?.Select(x => x.Id).ToList(),
                // ScaleHomeFavs = from.ScaleHomeFavs?.Select(x => x.Id).ToList(),


                //Store product
                Price = store.Price,
                Price2 = store.Price2,
                Cost = store.Cost,
            };
        }


        #region Conditional text

        public string Text1 { get; set; }
        public string Text2 { get; set; }
        public string Text3 { get; set; }
        public string Text4 { get; set; }

        #endregion Conditional text

        #region Tare
        public double Tare2 { get; set; }
        public bool ForceTare { get; set; }
        public bool TareIsPercent { get; set; }

        #endregion Tare


        #region Nutricional

        /// <summary>
        ///     Servings per container
        /// </summary>
        public double ServingPerContainer { get; set; }

        /// <summary>
        ///     Serving Size
        /// </summary>
        public double ServingSize { get; set; }

        /// <summary>
        ///     Amount per serving calories
        /// </summary>
        public double AmountPerServingCalories { get; set; }

        /// <summary>
        ///     Total fat grams(g)
        /// </summary>
        public double TotalFatGrams { get; set; }

        /// <summary>
        ///     Total fat %
        /// </summary>
        public double TotalFat { get; set; }

        /// <summary>
        ///     Saturated fat grams(g)
        /// </summary>
        public double SaturedFatGrams { get; set; }

        /// <summary>
        ///     Saturated fat %
        /// </summary>
        public double SaturedFat { get; set; }

        /// <summary>
        ///     Cholesterol mgrams(mg)
        /// </summary>
        public double CholesterolMGrams { get; set; }

        /// <summary>
        ///     Cholesterol %
        /// </summary>
        public double Cholesterol { get; set; }

        /// <summary>
        ///     Sodium mgrams(mg)
        /// </summary>
        public double SodiumMGrams { get; set; }

        /// <summary>
        ///     Sodium %
        /// </summary>
        public double Sodium { get; set; }

        /// <summary>
        ///     Total Carbohydrate grams(g)
        /// </summary>
        public double TotalCarbohydrateGrams { get; set; }

        /// <summary>
        ///     Total Carbohydrate %
        /// </summary>
        public double TotalCarbohydrate { get; set; }

        /// <summary>
        ///     Dietary Fiber grams(g)
        /// </summary>
        public double DietaryFiberGrams { get; set; }

        /// <summary>
        ///     Dietary Fiber %
        /// </summary>
        public double DietaryFiber { get; set; }

        /// <summary>
        ///     Total Sugar grams(g)
        /// </summary>
        public double TotalSugarGrams { get; set; }

        /// <summary>
        ///     Added Sugar grams(g)
        /// </summary>
        public double AddedSugarGrams { get; set; }

        /// <summary>
        ///     Added Sugar %
        /// </summary>
        public double AddedSugar { get; set; }

        /// <summary>
        ///     Protein grams (g)
        /// </summary>
        public double ProteinGrams { get; set; }

        /// <summary>
        ///     Vit. D microgram(mcg)
        /// </summary>
        public double VitDMGrams { get; set; }

        /// <summary>
        ///     Vit. D %
        /// </summary>
        public double VitD { get; set; }

        /// <summary>
        ///     Calcium mgrams(mg)
        /// </summary>
        public double CalciumMGrams { get; set; }

        /// <summary>
        ///     Calcium %
        /// </summary>
        public double Calcium { get; set; }

        /// <summary>
        ///     Iron mgrams(mg)
        /// </summary>
        public double IronMGrams { get; set; }

        /// <summary>
        ///     Iron %
        /// </summary>
        public double Iron { get; set; }

        /// <summary>
        ///     Potas mgrams(mg)
        /// </summary>
        public double PotasMGrams { get; set; }

        /// <summary>
        ///     Potas %
        /// </summary>
        public double Potas { get; set; }

        #endregion Nutricional
    }
}