using System;
using Greta.BO.Api.Entities.Enum;

namespace Greta.BO.Api.Entities
{
    public class VendorProduct : BaseEntityLong
    {
        public string ProductCode { get; set; }
        public bool IsPrimary { get; set; }
        public string PackSize { get; set; }
        public int CasePack { get; set; }
        public decimal CaseCost { get; set; }
        public decimal UnitCost { get; set; }
        public VendorProductType OrderByCase { get; set; }
        public DateTime? LastOrderDate { get; set; }

        public long VendorId { get; set; }
        public long ProductId { get; set; }

        public Vendor Vendor { get; set; }
        public Product Product { get; set; }
    }
}