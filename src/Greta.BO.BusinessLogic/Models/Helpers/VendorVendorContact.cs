using System.Collections.Generic;
using Greta.BO.Api.Entities;

namespace Greta.BO.BusinessLogic.Models.Helpers
{
    public class VendorVendorContact
    {
        public Vendor Vendor { get; set; }
        public List<VendorContactImage> VendorContactImages { get; set; }
    }
}