using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;

namespace Greta.BO.BusinessLogic.Models.Dto;

public class DepartmentExportModel: IMapFrom<Department>
{
    public int DepartmentId { get; set; }
    public string Name { get; set; }
    public bool Perishable { get; set; }
    public string BackgroundColor { get; set; }
    public string ForegroundColor { get; set; }
    public long Id { get; set; }
}