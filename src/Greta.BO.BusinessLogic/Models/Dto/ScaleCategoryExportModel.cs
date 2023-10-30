using AutoMapper;
using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;

namespace Greta.BO.BusinessLogic.Models.Dto;

public class ScaleCategoryExportModel:  IMapFrom<ScaleCategory>
{
    public string Name { get; set; }
    public long DepartmentId { get; set; }
    public int CategoryId { get; set; }
    public long? ParentId { get; set; }
    public string BackgroundColor { get; set; }
    public string ForegroundColor { get; set; }
    public long Id { get; set; }
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<ScaleCategory, ScaleCategoryExportModel>()
            .ForMember(vm => vm.DepartmentId, m => m.MapFrom(u => u.Department != null ? u.Department.DepartmentId : 0))
            .ReverseMap();
    }
}