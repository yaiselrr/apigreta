using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;

namespace Greta.BO.BusinessLogic.Models.Dto
{
    public class PermissionModel : IMapFrom<Permission>
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public string Code { get; set; }
    }
}