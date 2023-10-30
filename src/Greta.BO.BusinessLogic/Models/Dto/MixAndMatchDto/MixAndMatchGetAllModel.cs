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

public class MixAndMatchGetAllModel : IDtoLong<string>, IMapFrom<MixAndMatch>
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

    //----Of the fields between the lines, value showing the names in case they appear in the front

    public long? ProductBuyId { get; set; }
    public List<long> FamilyIds { get; set; }
    public List<long> ProductIds { get; set; }

    //-----------------------------------------------------------

    public bool State { get; set; }
    public string UserCreatorId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public long Id { get; set; }

}

