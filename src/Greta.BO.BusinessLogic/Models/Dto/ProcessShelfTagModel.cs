namespace Greta.BO.BusinessLogic.Models.Dto
{
    public class ProcessShelfTagModel
    {
        public long StoreId { get; set; }
        public long TagId { get; set; }
        public long? DepartmentId { get; set; }
        public long? CategoryId { get; set; }
        public long? BinLocationId { get; set; }
        public long? VendorId { get; set; }
    }
}