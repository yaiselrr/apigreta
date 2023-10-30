#nullable enable
using System.Collections.Generic;

namespace Greta.BO.Api.Entities
{
    public class QtyHandTrack : BaseEntityLong
    {
        public long ProductId { get; set; }
        
        public long StoreId { get; set; }

        public long DepartmentId { get; set; }
        
        public long CategoryId { get; set; }
        
        public long? ScaleCategoryId { get; set; }

        public string ProductName { get; set; }
        
        public string UPC { get; set; }
        
        public string StoreName { get; set; }
        
        public string DepartmentName { get; set; }
        
        public string CategoryName { get; set; }
        
        public string? ScaleCategoryName { get; set; }
        
        public decimal OldQtyHand { get; set; }
        
        public decimal NewQtyHand { get; set; }
        
        public string Username { get; set; }
        
        public string UserId { get; set; }
    }
}