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

public class MixAndMatchModel : IDtoLong<string>, IMapFrom<MixAndMatch>
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

    public long? ProductBuyId { get; set; }

    /// <summary>
    ///     Product to buy for activate the BuyOneGetFree
    /// </summary>
    public Product ProductBuy { get; set; }

    public List<long> FamilyIds { get; set; }
    public List<long> ProductIds { get; set; }
    public List<FamilyModel> Families { get; set; }
    public List<ProductModel> Products { get; set; }

    public bool State { get; set; }
    public string UserCreatorId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public long Id { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<MixAndMatch, MixAndMatchModel>().ReverseMap()
            .ForMember(vm => vm.Products, m => m.MapFrom(u => u.ProductIds.Select(x => new Product { Id = x })))
            .ForMember(vm => vm.Families, m => m.MapFrom(u => u.FamilyIds.Select(x => new Family { Id = x })));
    }
}