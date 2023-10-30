namespace Greta.BO.Api.Entities
{
    public class VendorContact : BaseEntityLong
    {
        public string Contact { get; set; }
        public string Phone { get; set; }
        public bool Primary { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public long VendorId { get; set; }
        public Vendor Vendor { get; set; }
    }
}