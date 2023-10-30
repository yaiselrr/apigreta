using System;
using System.ComponentModel.DataAnnotations;
using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;
using Greta.Sdk.Core.Abstractions;

namespace Greta.BO.BusinessLogic.Models.Dto
{
    public class ShelfTagModel : IDtoLong<string>, IMapFrom<ShelfTag>
    {
        public long Id { get; set; }
        
        public long ProductId { get; set; }
        public string ProductName { get; set; }

        public int QTYToPrint { get; set; }

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

        public bool State { get; set; }
        public string UserCreatorId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}