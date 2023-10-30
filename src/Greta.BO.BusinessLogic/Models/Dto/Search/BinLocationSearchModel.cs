using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;

namespace Greta.BO.BusinessLogic.Models.Dto.Search
{
    public class BinLocationSearchModel : BaseSearchModel, IMapFrom<BinLocation>
    {
        public string Name { get; set; }
        public long Store { get; set; }
    }
}