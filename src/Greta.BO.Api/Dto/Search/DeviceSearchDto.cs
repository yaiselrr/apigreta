using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;
using Greta.BO.BusinessLogic.Models.Dto.Search;

namespace Greta.BO.Api.Dto.Search
{
    public class DeviceSearchDto : BaseSearchDto, IMapFrom<DeviceSearchModel>
    {
        public string Name { get; set; }
    }
}