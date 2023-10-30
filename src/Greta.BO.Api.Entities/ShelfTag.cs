using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Greta.BO.Api.Entities.Enum;

namespace Greta.BO.Api.Entities
{
    public class ShelfTag : BaseEntityLong
    {
        public int QTYToPrint { get; set; }
        public long ProductId { get; set; }
        public string ProductName { get; set; }
        public string UPC { get; set; }

        public long StoreId { get; set; }
        public string StoreName { get; set; }
        
        public decimal Price { get; set; }
        
        public long CategoryId { get; set; }
        public string CategoryName { get; set; }
        
        public long DepartmentId { get; set; }
        public string DepartmentName { get; set; }

        public long VendorId { get; set; }
        public string VendorName { get; set; }
        public string ProductCode { get; set; }
        public int CasePack { get; set; }

        public long BinLocationId { get; set; }
        public string BinLocationName { get; set; }
    }
}