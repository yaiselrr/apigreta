
using AutoMapper;
using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;

namespace Greta.BO.BusinessLogic.Models.Dto;

public class CategoryExportModel: IMapFrom<Category>
{
    public int CategoryId { get; set; }
    
    public string Name { get; set; }
    public string Description { get; set; }
    public long DepartmentId { get; set; }
    public bool VisibleOnPos { get; set; }
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
    public string BackgroundColor { get; set; }
    public string ForegroundColor { get; set; }
    public long Id { get; set; }


    public void Mapping(Profile profile)
    {
        profile.CreateMap<Category, CategoryExportModel>()
            .ForMember(vm => vm.MinimumAge, m => m.MapFrom(u => u.MinimumAge == -1 ? null : u.MinimumAge))
            .ForMember(vm => vm.DepartmentId, m => m.MapFrom(u => u.Department != null ? u.Department.DepartmentId : 0))
            .ReverseMap();
    }
}