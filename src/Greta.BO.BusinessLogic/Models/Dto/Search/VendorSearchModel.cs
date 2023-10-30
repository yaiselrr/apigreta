using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;

namespace Greta.BO.BusinessLogic.Models.Dto.Search
{
    public class VendorSearchModel : BaseSearchModel, IMapFrom<Vendor>
    {
        public string Name { get; set; }
        public string AccountNumber { get; set; }

        // public long CityId { get; set; }
        // public long ProvinceId { get; set; }
        // public long CountryId { get; set; }
    }
}