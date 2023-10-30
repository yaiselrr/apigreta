using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AutoMapper;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;
using Greta.Sdk.Core.Abstractions;

namespace Greta.BO.BusinessLogic.Models.Dto
{
    public class FeeGetByIdModel : IDtoLong<string>, IMapFrom<Fee>
    {
        [Required]
        [StringLength(30, ErrorMessage = "The {0} field not is valid")]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required] public decimal Amount { get; set; }

        [Required] public DiscountType Type { get; set; }

        public bool IncludeInItemPrice { get; set; }
        public bool ApplyDiscount { get; set; }
        public bool ApplyFoodStamp { get; set; }
        public bool ApplyAutomatically { get; set; }
        public bool ApplyToItemQty { get; set; }
        public bool ApplyTax { get; set; }
        public bool RestrictToItems { get; set; }


        public List<long> FamilyIds { get; set; }
        public List<long> CategoryIds { get; set; }
        public List<long> ProductIds { get; set; }

        public List<FamilyModel> Families { get; set; }
        public List<CategoryModel> Categories { get; set; }
        public List<ProductForFeeModel> Products { get; set; }


        public long Id { get; set; }
        public bool State { get; set; }
        public string UserCreatorId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Fee, FeeGetByIdModel>().ReverseMap()
                .ForMember(vm => vm.Products,
                    m => m.MapFrom(u => u.ProductIds.Select(x => new Product { Id = x })))
                .ForMember(vm => vm.Categories,
                    m => m.MapFrom(u => u.CategoryIds.Select(x => new Category { Id = x })))
                .ForMember(vm => vm.Families,
                    m => m.MapFrom(u => u.FamilyIds.Select(x => new Family { Id = x })));
        }
    }

    public class CategoryForFeeModel : IDtoLong<string>, IMapFrom<Category>
    {
        [Required] public int CategoryId { get; set; }

        [Required]
        [StringLength(64, ErrorMessage = "The {0} field not is valid")]
        public string Name { get; set; }

        [Required]
        [StringLength(254, ErrorMessage = "The {0} field not is valid")]
        public string Description { get; set; }

        [Required] public long DepartmentId { get; set; }
        [Required] public bool VisibleOnPos { get; set; }

        public long? DefaulShelfTagId { get; set; }

        public ScaleLabelTypeModel DefaulShelfTag { get; set; }


        public bool PromptPriceAtPOS { get; set; }
        public bool SnapEBT { get; set; }
        public bool PrintShelfTag { get; set; }
        public bool NoPriceOnShelfTag { get; set; }
        public bool AllowZeroStock { get; set; }
        public int? MinimumAge { get; set; }
        public bool NoDiscountAllowed { get; set; }
        public bool AddOnlineStore { get; set; }
        public bool Modifier { get; set; }
        public bool DisplayStockOnPosButton { get; set; }


        public DepartmentModel Department { get; set; }
        public string BackgroundColor { get; set; }
        public string ForegroundColor { get; set; }
        public List<long> TaxsIds { get; set; }

        public long Id { get; set; }
        public bool State { get; set; }
        public string UserCreatorId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }

    public class ProductForFeeModel : IDtoLong<string>, IMapFrom<Product>
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
        
        public long Id { get; set; }
        public bool State { get; set; }
        public string UserCreatorId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

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

        #endregion

    }

}