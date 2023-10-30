using Greta.Sdk.EFCore.Interfaces;
using System.Collections.Generic;

namespace Greta.BO.Api.Entities
{
    public class Vendor : BaseLocationEntityLong, IFullSyncronizable
    {
        public string Name { get; set; }
        public string AccountNumber { get; set; }
        public string Note { get; set; }
        public double MinimalOrder { get; set; }

        public List<VendorContact> VendorContacts { get; set; }
        public List<VendorProduct> VendorProducts { get; set; }
        public virtual List<VendorOrder> VendorOrders { get; set; }
    }
}