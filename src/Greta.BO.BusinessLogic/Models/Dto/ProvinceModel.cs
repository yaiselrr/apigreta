using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;

namespace Greta.BO.BusinessLogic.Models.Dto
{
    public class ProvinceModel : IMapFrom<Province>
    {
        public long Id { get; set; }

        public string Name { get; set; }
    }
}