namespace Greta.BO.Api.Entities;

public class OnlineCategory: BaseEntityLong
{
    public long OnlineStoreId { get; set; }
    public long CategoryId { get; set; }
    public string OnlineCategoryId { get; set; }

    public Category Category { get; set; }
    public OnlineStore OnlineStore { get; set; }
}

public class OnlineProduct: BaseEntityLong
{
    public long OnlineStoreId { get; set; }
    public long ProductId { get; set; }
    public string OnlineProductId { get; set; }
    
    public StoreProduct Product { get; set; }
    public OnlineStore OnlineStore { get; set; }
}