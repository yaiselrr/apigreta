using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Greta.BO.Api.Entities.Enum;
using Greta.Sdk.Core.Abstractions;

namespace Greta.BO.BusinessLogic.Models.Dto
{
    public class KitProductModel : IDtoLong<string>
    {
        [Required] public string UPC { get; set; }

        [Required]
        [StringLength(64, ErrorMessage = "The {0} field not is valid")]
        public string Name { get; set; }

        [StringLength(254, ErrorMessage = "The {0} field not is valid")]
        public string Description { get; set; }

        [Required] public ProductType ProductType { get; set; }

        public int MinimumAge { get; set; }
        public bool PosVisible { get; set; }
        public bool ScaleVisible { get; set; }
        public bool AllowZeroStock { get; set; }

        [Required] public long CategoryId { get; set; }

        [Required] public long FamilyId { get; set; }

        [Required] public long DepartmentId { get; set; }

        public DepartmentModel Department { get; set; }
        public CategoryModel Category { get; set; }
        public FamilyModel Family { get; set; }

        public List<VendorProductModel> VendorProducts { get; set; }

        public List<KitProductModel> KitProducts { get; set; }

        [Required] public List<long> ProductsId { get; set; }

        public List<ProductModel> Products { get; set; }
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

        #endregion Inventory
    }
}