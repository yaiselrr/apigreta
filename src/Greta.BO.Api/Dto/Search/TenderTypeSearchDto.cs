using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;
using Greta.BO.BusinessLogic.Models.Dto.Search;

namespace Greta.BO.Api.Dto.Search
{
    public class TenderTypeSearchDto : BaseSearchDto, IMapFrom<TenderTypeSearchModel>
    {
        public string Name { get; set; }
        public string DisplayAs { get; set; }
        public bool PaymentGateway { get; set; }
    }
}