using Greta.BO.Api.Entities;
using Greta.BO.Api.Entities.Enum;

namespace Greta.BO.Api.Test.ExportImport
{
    public class FakeProductData
    {
        public static Product ValidProduct => new Product
        {
            UPC = "12345", Name = "Product 1", CategoryId = 1, DepartmentId = 1, FamilyId = 1, DefaulShelfTagId = 1, 
            ProductType = ProductType.SPT,  MinimumAge = 0, PosVisible = false, ScaleVisible = true, 
            AllowZeroStock = true, NoDiscountAllowed = false, PromptPriceAtPOS = false, SnapEBT = false, 
            PrintShelfTag = false, NoPriceOnShelfTag = false, AddOnlineStore = true, Modifier = false, 
            LoyaltyPoints = 1, DisplayStockOnPosButton = false
        };

        public static Product InValidProductUPCDouble => new Product
        {
            UPC = "12345", Name = "Product 2", CategoryId = 1, DepartmentId = 1, FamilyId = 1, DefaulShelfTagId = 1,
            ProductType = ProductType.SPT, MinimumAge = 0, PosVisible = false, ScaleVisible = true,
            AllowZeroStock = true, NoDiscountAllowed = false, PromptPriceAtPOS = false, SnapEBT = false,
            PrintShelfTag = false, NoPriceOnShelfTag = false, AddOnlineStore = true, Modifier = false,
            LoyaltyPoints = 1, DisplayStockOnPosButton = false
        };

        public static Product InValidProductEmptyName => new Product
        {
            UPC = "12345", Name = "", CategoryId = 1, DepartmentId = 1, FamilyId = 1, DefaulShelfTagId = 1,
            ProductType = ProductType.SPT, MinimumAge = 0, PosVisible = false, ScaleVisible = true,
            AllowZeroStock = true, NoDiscountAllowed = false, PromptPriceAtPOS = false, SnapEBT = false,
            PrintShelfTag = false, NoPriceOnShelfTag = false, AddOnlineStore = true, Modifier = false,
            LoyaltyPoints = 1, DisplayStockOnPosButton = false
        };
        public static Product InValidProductCatId0 => new Product
        {
            UPC = "12345", Name = "Product 4", CategoryId = 0, DepartmentId = 1, FamilyId = 1, DefaulShelfTagId = 1, 
            ProductType = ProductType.SPT,  MinimumAge = 0, PosVisible = false, ScaleVisible = true, 
            AllowZeroStock = true, NoDiscountAllowed = false, PromptPriceAtPOS = false, SnapEBT = false, 
            PrintShelfTag = false, NoPriceOnShelfTag = false, AddOnlineStore = true, Modifier = false, 
            LoyaltyPoints = 1, DisplayStockOnPosButton = false
        };
        public static Product InValidProductDptId0 => new Product
        {
            UPC = "12345", Name = "Product 5", CategoryId = 1, DepartmentId = 0, FamilyId = 1, DefaulShelfTagId = 1, 
            ProductType = ProductType.SPT,  MinimumAge = 0, PosVisible = false, ScaleVisible = true, 
            AllowZeroStock = true, NoDiscountAllowed = false, PromptPriceAtPOS = false, SnapEBT = false, 
            PrintShelfTag = false, NoPriceOnShelfTag = false, AddOnlineStore = true, Modifier = false, 
            LoyaltyPoints = 1, DisplayStockOnPosButton = false
        };
        public static Product InValidProductCatIdAndDptId0 => new Product
        {
            UPC = "12345", Name = "Product 6", CategoryId = 0, DepartmentId = 0, FamilyId = 1, DefaulShelfTagId = 1, 
            ProductType = ProductType.SPT,  MinimumAge = 0, PosVisible = false, ScaleVisible = true, 
            AllowZeroStock = true, NoDiscountAllowed = false, PromptPriceAtPOS = false, SnapEBT = false, 
            PrintShelfTag = false, NoPriceOnShelfTag = false, AddOnlineStore = true, Modifier = false, 
            LoyaltyPoints = 1, DisplayStockOnPosButton = false
        };
    }
}