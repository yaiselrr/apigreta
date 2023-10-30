using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;

namespace Greta.BO.BusinessLogic.Models.Dto.Search
{
    public class ScaleHomeFavSearchModel : BaseSearchModel, IMapFrom<ScaleHomeFav>
    {
        public long DepartmentId { get; set; }

        public long StoreId { get; set; }
    }
}