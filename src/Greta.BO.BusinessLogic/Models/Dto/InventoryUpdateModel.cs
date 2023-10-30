using System.ComponentModel.DataAnnotations;

namespace Greta.BO.BusinessLogic.Models.Dto
{
    public class InventoryUpdateModel
    {
        [Required]
        public long StoreProductId{ get; set; }
        
        public long BinLocationId { get; set; }
        public decimal? QtyHand { get; set; }
        public decimal? OrderTrigger { get; set; }
        public decimal? OrderAmount { get; set; }
    }
}