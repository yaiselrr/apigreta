using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;
using Greta.BO.BusinessLogic.Models.Dto.Search;

namespace Greta.BO.Api.Dto.Search
{
    public class ProfilesSearchDto : BaseSearchDto, IMapFrom<ProfilesSearchModel>
    {
        public string Name { get; set; }

        public long ApplicationId { get; set; }
    }
}