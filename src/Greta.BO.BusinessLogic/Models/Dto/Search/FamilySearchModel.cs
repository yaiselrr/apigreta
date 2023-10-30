using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;

namespace Greta.BO.BusinessLogic.Models.Dto.Search
{
    public class FamilySearchModel : BaseSearchModel, IMapFrom<Family>
    {
        public string Name { get; set; }
        public long FamilyId { get; set; }
    }
}