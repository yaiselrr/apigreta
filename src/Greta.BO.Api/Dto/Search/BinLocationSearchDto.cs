using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;
using Greta.BO.BusinessLogic.Models.Dto.Search;

namespace Greta.BO.Api.Dto.Search
{
    public class BinLocationSearchDto : BaseSearchDto, IMapFrom<BinLocationSearchModel>
    {
        public string Name { get; set; }
        public long Store { get; set; }
    }
}