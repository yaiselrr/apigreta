
using Greta.BO.Api.Entities.Entensions;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.Api.Entities.Helpers;
using Greta.BO.Api.Entities.Lite;

var deps = new List<LiteDepartment>()
{
    new LiteDepartment
    {
        Id = 1,
        State = true,
        UserCreatorId = "UserCreatorId",
        CreatedAt = DateTime.Now,
        UpdatedAt = DateTime.Now,
        DepartmentId = 1,
        Name = "DEP1",
        Perishable = false,
        BackgroundColor = null,
        ForegroundColor = null
    },
    new LiteDepartment
    {
        Id = 2,
        State = true,
        UserCreatorId = "UserCreatorId",
        CreatedAt = DateTime.Now,
        UpdatedAt = DateTime.Now,
        DepartmentId = 2,
        Name = "DEP2",
        Perishable = false,
        BackgroundColor = null,
        ForegroundColor = null
    }
};

var tax = new LiteTax
{
    Id = 1,
    State = true,
    UserCreatorId = "null",
    CreatedAt = DateTime.Now,
    UpdatedAt = DateTime.Now,
    Name = "Tax 1",
    Description = null,
    Type = TaxType.FIX,
    Value = 3,
    SpecialValue = 1
};

var cats = new List<LiteCategory>()
{
    new LiteCategory
    {
        Id = 1,
        State = true,
        UserCreatorId = "null",
        CreatedAt = DateTime.Now,
        UpdatedAt = DateTime.Now,
        CategoryId = 1,
        Name = "Cat1",
        Description = "null",
        DepartmentId = 1,
        VisibleOnPos = true,
        BackgroundColor = null,
        ForegroundColor = null,
        Taxes = null
    },
    new LiteCategory
    {
        Id = 2,
        State = true,
        UserCreatorId = "null",
        CreatedAt = DateTime.Now,
        UpdatedAt = DateTime.Now,
        CategoryId = 2,
        Name = "Cat2",
        Description = "null",
        DepartmentId = 1,
        VisibleOnPos = true,
        BackgroundColor = null,
        ForegroundColor = null,
        Taxes = new List<long>(){1}
    }
};
var products = new List<LiteProduct>()
{
    new LiteProduct
    {
        Id = 1,
        State = true,
        UserCreatorId = "null",
        CreatedAt = DateTime.Now,
        UpdatedAt = DateTime.Now,
        UPC = "upc1",
        Name = "nAME1",
        CategoryId = 1,
        DepartmentId = 1,
        FamilyId = null,
        DefaulShelfTagId = null,
        ProductType = ProductType.SPT,
        MinimumAge = null,
        PosVisible = false,
        ScaleVisible = false,
        AllowZeroStock = false,
        NoDiscountAllowed = false,
        PromptPriceAtPOS = false,
        SnapEBT = false,
        PrintShelfTag = false,
        NoPriceOnShelfTag = false,
        AddOnlineStore = false,
        Modifier = false,
        LoyaltyPoints = 0,
        DisplayStockOnPosButton = false,
        IsWeighted = false,
        Tare1 = 0,
        Price = 0,
        Price2 = 0,
        Cost = 0,
    },
    new LiteProduct
    {
        Id = 2,
        State = true,
        UserCreatorId = "null",
        CreatedAt = DateTime.Now,
        UpdatedAt = DateTime.Now,
        UPC = "upc2",
        Name = "nAME2",
        CategoryId = 1,
        DepartmentId = 1,
        FamilyId = null,
        DefaulShelfTagId = null,
        ProductType = ProductType.SPT,
        MinimumAge = null,
        PosVisible = false,
        ScaleVisible = false,
        AllowZeroStock = false,
        NoDiscountAllowed = false,
        PromptPriceAtPOS = false,
        SnapEBT = false,
        PrintShelfTag = false,
        NoPriceOnShelfTag = false,
        AddOnlineStore = false,
        Modifier = false,
        LoyaltyPoints = 0,
        DisplayStockOnPosButton = false,
        IsWeighted = false,
        Tare1 = 0,
        Price = 0,
        Price2 = 0,
        Cost = 0,
    }
};


var discount = new LiteDiscount
{
    Id = 1,
    State = true,
    UserCreatorId = "null",
    CreatedAt = DateTime.Now,
    UpdatedAt = DateTime.Now,
    Name = "Disc1",
    Type = DiscountType.FIX,
    Value = 8,
    DepartmentId = null,
    CategoryId = null,
    Products = new List<long>(){ 1, 2}
};
var scaleType = new LiteScaleLabelType
{
    Id = 1,
    State = true,
    UserCreatorId = "QWERTYUI",
    CreatedAt = DateTime.Now,
    UpdatedAt = DateTime.Now,
    Name = "Label",
    LabelId = 1,
    ScaleType = ScaleType.SHELFTAG,
    Design = "scscs"
};

var scaleproduct = new LiteScaleProduct()
{
    Id = 2,
    State = true,
    UserCreatorId = "null",
    CreatedAt = DateTime.Now,
    UpdatedAt = DateTime.Now,
    UPC = "upc2",
    Name = "nAME2",
    CategoryId = 1,
    DepartmentId = 1,
    FamilyId = null,
    DefaulShelfTagId = null,
    ProductType = ProductType.SPT,
    MinimumAge = null,
    PosVisible = false,
    ScaleVisible = false,
    AllowZeroStock = false,
    NoDiscountAllowed = false,
    PromptPriceAtPOS = false,
    SnapEBT = false,
    PrintShelfTag = false,
    NoPriceOnShelfTag = false,
    AddOnlineStore = false,
    Modifier = false,
    LoyaltyPoints = 0,
    DisplayStockOnPosButton = false,
    IsWeighted = false,
    Tare1 = 20,
    Price = 0,
    Price2 = 0,
    Cost = 0,
    Tare2 = 25,
};


SqlQueryBuilder builder = new SqlQueryBuilder();

scaleproduct.ToSqlQuery(ref builder, SqlOperationType.InsertOrUpdate);
scaleproduct.ToSqlQuery(ref builder, SqlOperationType.Delete);
// scaleType.ToSqlQuery(ref builder, SqlOperationType.InsertOrUpdate);
// foreach (var d in deps)
// {
//     d.ToSqlQuery(ref builder, SqlOperationType.InsertOrUpdate);
// }
//
// tax.ToSqlQuery(ref builder, SqlOperationType.InsertOrUpdate);
//
// foreach (var c in cats)
// {
//     c.ToSqlQuery(ref builder, SqlOperationType.InsertOrUpdate);
// }
//
// foreach (var p in products)
// {
//     p.ToSqlQuery(ref builder, SqlOperationType.InsertOrUpdate);
// }
//
// discount.ToSqlQuery(ref builder, SqlOperationType.InsertOrUpdate);

var query = builder.SaveString("Partial", "6");
Console.WriteLine(query);