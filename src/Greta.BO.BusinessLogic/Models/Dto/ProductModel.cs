using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;
using Greta.Sdk.Core.Abstractions;

namespace Greta.BO.BusinessLogic.Models.Dto
{
    public class ProductModel : IMapFrom<Product>, IDtoLong<string>
    {
        [Required] public string UPC { get; set; }

        [Required]
        [StringLength(64, ErrorMessage = "The {0} field not is valid")]
        public string Name { get; set; }

        [Required] public ProductType ProductType { get; set; }

        public long? DefaulShelfTagId { get; set; }

        public int MinimumAge { get; set; }
        public bool PosVisible { get; set; }
        public bool ScaleVisible { get; set; }
        public bool AllowZeroStock { get; set; }

        public bool NoDiscountAllowed { get; set; }

        public bool PromptPriceAtPOS { get; set; }

        public bool SnapEBT { get; set; }

        public bool PrintShelfTag { get; set; }

        public bool NoPriceOnShelfTag { get; set; }

        public bool AddOnlineStore { get; set; }

        public bool Modifier { get; set; }

        public int LoyaltyPoints { get; set; }

        [Required] public long CategoryId { get; set; }

        public long? FamilyId { get; set; }

        [Required] public long DepartmentId { get; set; }

        public ScaleLabelTypeModel DefaulShelfTag { get; set; }
        public CategoryModel Category { get; set; }
        public FamilyModel Family { get; set; }

        public DepartmentModel Department { get; set; }

        public List<VendorProductModel> VendorProducts { get; set; }

        public List<StoreProductModel> StoreProducts { get; set; }

        public List<KitProductModel> KitProducts { get; set; }
        public long Id { get; set; }
        public bool State { get; set; }
        public string UserCreatorId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public bool IsWeighted { get; set; }
        public decimal Tare1 { get; set; }
        #region Inventory

        /// <summary>
        ///     Qty on Hand
        /// </summary>
        public int QtyHand { get; set; }

        /// <summary>
        ///     Not defined yet
        /// </summary>
        public int OrderTrigger { get; set; }

        /// <summary>
        ///     Not defined yet
        /// </summary>
        public int OrderAmount { get; set; }

        #endregion Inventory
    }
    
    public class ProductExportModel : IMapFrom<Product>
    {
        public string UPC { get; set; }
        public string Name { get; set; }
        public int? MinimumAge { get; set; }
        public bool PosVisible { get; set; }
        public bool ScaleVisible { get; set; }
        public bool AllowZeroStock { get; set; }
        public bool NoDiscountAllowed { get; set; }
        public bool PromptPriceAtPOS { get; set; }
        public bool SnapEBT { get; set; }
        public bool PrintShelfTag { get; set; }
        public bool NoPriceOnShelfTag { get; set; }
        public bool AddOnlineStore { get; set; }
        public bool Modifier { get; set; }
        public int LoyaltyPoints { get; set; }
        public long CategoryId { get; set; }
        public long DepartmentId { get; set; }
        public long Id { get; set; }
        public decimal QtyHand { get; set; }
        public decimal Price { get; set; }
        public decimal Cost { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Product, ProductExportModel>()
                .ForMember(vm => vm.MinimumAge, m => m.MapFrom(u => u.MinimumAge != -1 ? u.MinimumAge : null))
                .ForMember(vm => vm.DepartmentId, m => m.MapFrom(u => u.Department != null ? u.Department.DepartmentId : 0))
                .ForMember(vm => vm.CategoryId, m => m.MapFrom(u => u.Category != null ? u.Category.CategoryId : 0))
                .ReverseMap();
        }
    }
}