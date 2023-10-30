namespace Greta.BO.BusinessLogic.Models.Dto.Search
{
    public class VendorOrderDetailSearchModel: BaseSearchModel
    {
        public string ProductName { get; set; }
        public long StoreId { get; set; }
        public long HeaderId { get; set; }
    }
}