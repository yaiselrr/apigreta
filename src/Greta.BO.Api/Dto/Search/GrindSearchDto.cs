using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;
using Greta.BO.BusinessLogic.Models.Dto.Search;

namespace Greta.BO.Api.Dto.Search
{
    public class GrindSearchDto : BaseSearchDto, IMapFrom<GrindSearchModel>
    {
        public string Name { get; set; }
    }
}