using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;

namespace Greta.BO.BusinessLogic.Models.Dto.Search
{
    public class ExternalScaleSearchModel : BaseSearchModel, IMapFrom<ExternalScale>
    {
        public string Ip { get; set; }

        public long StoreId { get; set; }

        public long ScaleBrandId { get; set; }
    }
}