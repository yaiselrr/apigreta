using Greta.BO.Api.Entities.Enum;
using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;
using Greta.BO.BusinessLogic.Models.Dto.Search;

namespace Greta.BO.Api.Dto.Search
{
    public class ColumnNameSearchDto : BaseSearchDto, IMapFrom<ColumnNameSearchModel>
    {

        public bool Imported { get; set; }
        public string Name { get; set; }
        public ModelImport modelImport { get; set; }
    }
}