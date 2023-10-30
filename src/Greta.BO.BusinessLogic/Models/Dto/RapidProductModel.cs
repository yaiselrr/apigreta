using System;
using System.ComponentModel.DataAnnotations;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;
using Greta.Sdk.Core.Abstractions;

namespace Greta.BO.BusinessLogic.Models.Dto
{
    public class RapidProductModel : IDtoLong<string>, IMapFrom<ProductModel>
    {
        [Required]
        [StringLength(24, ErrorMessage = "The {0} field not is valid")]
        public string ProductCode { get; set; }
        [Required] public ProductType ProductType { get; set; }
        [Required] public string UPC { get; set; }
        [Required] public string Name { get; set; }       
        [Required] public long DepartmentId { get; set; }
        [Required] public long CategoryId { get; set; }
        [Required] public long StoreId { get; set; }
        [Required] public long VendorId { get; set; }
        public DepartmentModel Department { get; set; }
        public CategoryModel Category { get; set; }
        public StoreModel Store { get; set; }
        public VendorModel Vendor { get; set; }      
        [Required] public int CasePack { get; set; }
        [Required] public decimal CaseCost { get; set; }
        [Required] public decimal UnitCost { get; set; }
        [Required] public VendorProductType OrderByCase { get; set; }
        [Required] public decimal RetailPrice { get; set; }

        //From Category
        public bool PosVisible { get; set; }
        public long? DefaulShelfTagId { get; set; }
        public ScaleLabelTypeModel DefaulShelfTag { get; set; }
        public bool PromptPriceAtPOS { get; set; }
        public bool SnapEBT { get; set; }
        public bool PrintShelfTag { get; set; }
        public bool NoPriceOnShelfTag { get; set; }
        public int MinimumAge { get; set; }
        public bool AddOnlineStore { get; set; }
        public bool Modifier { get; set; }

        //--------------
        public long Id { get; set; }
        public bool State { get; set; }
        public string UserCreatorId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}