using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;
using Greta.BO.BusinessLogic.Models.Dto.Search;

namespace Greta.BO.Api.Dto.Search
{
    public class VendorSearchDto : BaseSearchDto, IMapFrom<VendorSearchModel>
    {
        public string Name { get; set; }
        public string AccountNumber { get; set; }

        // public long CityId { get; set; }
        // public long ProvinceId { get; set; }
        // public long CountryId { get; set; }
    }
}