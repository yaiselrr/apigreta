using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;

namespace Greta.BO.BusinessLogic.Models.Dto.Search
{
    public class DeviceSearchModel : BaseSearchModel, IMapFrom<Device>
    {
        public string Name { get; set; }
        public long StoreId { get; set; }
    }
}