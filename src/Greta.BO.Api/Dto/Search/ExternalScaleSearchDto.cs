using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;
using Greta.BO.BusinessLogic.Models.Dto.Search;

namespace Greta.BO.Api.Dto.Search
{
    public class ExternalScaleSearchDto : BaseSearchDto, IMapFrom<ExternalScaleSearchModel>
    {
        public string Ip { get; set; }

        public long StoreId { get; set; }

        public long ScaleBrandId { get; set; }
    }
}