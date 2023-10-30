using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AutoMapper;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;
using Greta.Sdk.Core.Abstractions;

namespace Greta.BO.BusinessLogic.Models.Dto.MixAndMatchDto;

public class MixAndMatchGetByIdModel : IDtoLong<string>, IMapFrom<MixAndMatch>
{
    [Required]
    [StringLength(64, ErrorMessage = "The {0} field not is valid")]
    public string Name { get; set; }

    public decimal Amount { get; set; }
    public int QTY { get; set; }
    public bool ActivePeriod { get; set; }
    public bool ApplyToCustomerOnly { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public MixAndMatchType MixAndMatchType { get; set; }

    public bool NotAllowAnyOtherDiscount { get; set; }

    //--------Of the fields between the lines, value their use in the returned result.
    //--------Of the models described below, evaluate the fields that are shown in the front

    public long? ProductBuyId { get; set; }

    /// <summary>
    ///     Product to buy for activate the BuyOneGetFree
    /// </summary>
    public ProductBy ProductBuy { get; set; }

    public List<long> FamilyIds { get; set; }
    public List<long> ProductIds { get; set; }
    public List<FamilyModel> Families { get; set; }
    public List<ProductSingleModel> Products { get; set; }

    //----------------------------------------------------------------

    public bool State { get; set; }
    public string UserCreatorId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public long Id { get; set; }
}

public class ProductBy : IMapFrom<Product>
{
    [Required] public string UPC { get; set; }

    [Required]
    [StringLength(64, ErrorMessage = "The {0} field not is valid")]
    public string Name { get; set; }

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


    public long Id { get; set; }
    public bool State { get; set; }
    public string UserCreatorId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class ProductSingleModel : IMapFrom<Product>
{
    [Required] public string UPC { get; set; }

    [Required]
    [StringLength(64, ErrorMessage = "The {0} field not is valid")]
    public string Name { get; set; }

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

    public long Id { get; set; }
    public bool State { get; set; }
    public string UserCreatorId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}