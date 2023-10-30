using Greta.BO.Api.Entities;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;

namespace Greta.BO.BusinessLogic.Models.Dto.Search
{
    public class ColumnNameSearchModel : BaseSearchModel, IMapFrom<Column.ColumnNameModel>
    {
        public bool Imported { get; set; }
        public string Name { get; set; }
        public ModelImport modelImport { get; set; }
    }
}