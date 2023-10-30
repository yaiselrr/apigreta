using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;
using Greta.BO.BusinessLogic.Models.Dto.Search;

namespace Greta.BO.Api.Dto.Search
{
    public class CSVMappingSearchDto : BaseSearchDto, IMapFrom<CSVMappingSearchModel>
    {
        public string Name { get; set; }
    }
}