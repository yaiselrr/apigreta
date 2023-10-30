using System;
using System.ComponentModel.DataAnnotations;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;
using Greta.Sdk.Core.Abstractions;

namespace Greta.BO.BusinessLogic.Models.Dto
{
    public class VendorProductModel : IDtoLong<string>, IMapFrom<VendorProduct>
    {
        [Required]
        [StringLength(24, ErrorMessage = "The {0} field not is valid")]
        public string ProductCode { get; set; }

        public bool IsPrimary { get; set; }
        
        public string PackSize { get; set; }
        [Required] public int CasePack { get; set; }

        [Required] public decimal CaseCost { get; set; }

        [Required] public decimal UnitCost { get; set; }

        [Required] public VendorProductType OrderByCase { get; set; }

        public DateTime? LastOrderDate { get; set; }

        public long VendorId { get; set; }
        public long ProductId { get; set; }

        public VendorModel Vendor { get; set; }
        public ProductModel Product { get; set; }

        public long Id { get; set; }
        public bool State { get; set; }
        public string UserCreatorId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}