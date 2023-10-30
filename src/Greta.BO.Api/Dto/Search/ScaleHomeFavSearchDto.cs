using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;
using Greta.BO.BusinessLogic.Models.Dto.Search;

namespace Greta.BO.Api.Dto.Search
{
    public class ScaleHomeFavSearchDto : BaseSearchDto, IMapFrom<ScaleHomeFavSearchModel>
    {
        public long DepartmentId { get; set; }

        public long StoreId { get; set; }
    }
}