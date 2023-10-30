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
    /// <summary>
    /// 
    /// </summary>
    public class CutListDetailListModel
    {
        public List<CutListDetailModel> Elements { get; set; }
        
        public long CutList { get; set; }
    }

    /// <inheritdoc />
    public class CutListDetailModel : IDtoLong<string>, IMapFrom<CutListDetail>
    {
        [Required]
        public long CutListId { get; set; }
        [Required]
        public long ProductId { get; set; }
        
        public CutListModel CutList { get; set; }
        public CutListDetailProductModel Product { get; set; }

        [Required]
        public int Pack { get; set; }
        [Required]
        public decimal Thick { get; set; }

        public long Id { get; set; }

        public bool State { get; set; }
        public string UserCreatorId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        
        /*
        public void Mapping(Profile profile)
        {
            
             profile.CreateMap<CutListDetail, CutListDetailModel>().ReverseMap()
                 .ForMember(vm => vm..CutListDetails, m => m.MapFrom(u => u.CutListDetailsIds.Select(x => new CutListDetail() {Id = x})));
        }
        */
    }

    /// <inheritdoc />
    public class CutListDetailProductModel: IMapFrom<Product>
    {
        public long Id { get; set; }
        public string UPC { get; set; }
        public string Name { get; set; }
    }
}