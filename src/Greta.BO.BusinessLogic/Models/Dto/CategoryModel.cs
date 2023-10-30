using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AutoMapper;
using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;
using Greta.Sdk.Core.Abstractions;

namespace Greta.BO.BusinessLogic.Models.Dto
{
    public class CategoryModel : IDtoLong<string>, IMapFrom<Category>
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
        
        public decimal TargetGrossProfit { get; set; }
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
        public bool IsLiquorCategory { get; set; }
        public List<long> TaxsIds { get; set; }
        public List<TaxModel> Taxs { get; set; }

        public long Id { get; set; }
        public bool State { get; set; }
        public string UserCreatorId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }


        public void Mapping(Profile profile)
        {
            profile.CreateMap<Category, CategoryModel>().ReverseMap()
                .ForMember(vm => vm.Taxs, m => m.MapFrom(u => u.TaxsIds.Select(x => new Tax {Id = x})));
        }
    }
}