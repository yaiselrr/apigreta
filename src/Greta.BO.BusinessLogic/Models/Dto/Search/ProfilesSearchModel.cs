using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;

namespace Greta.BO.BusinessLogic.Models.Dto.Search
{
    public class ProfilesSearchModel : BaseSearchModel, IMapFrom<Profiles>
    {
        public string Name { get; set; }
        public long ApplicationId { get; set; }
        public string Application { get; set; }
    }
}