

using System;
using System.Collections.Generic;

namespace Greta.BO.BusinessLogic.Models.Dto
{
    public class InventoryFiscalModel
    {
        public DateTime DateTime { get; set; }
        public long StoreId { get; set; }
        public List<InventoryFiscalItemModel> Items { get; set; }
    }

    public class InventoryFiscalItemModel
    {
        public long Id { get; set; }
        public string UPC { get; set; }
        public string Name { get; set; }
        public bool Update { get; set; }
        public decimal Count { get; set; }
        public decimal CountSold { get; set; }
        public decimal QtyHand { get; set; }
    }
}
