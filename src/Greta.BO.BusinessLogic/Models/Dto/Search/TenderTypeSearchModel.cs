using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;

namespace Greta.BO.BusinessLogic.Models.Dto.Search
{
    public class TenderTypeSearchModel : BaseSearchModel, IMapFrom<TenderType>
    {
        public string Name { get; set; }
        public string DisplayAs { get; set; }
        public bool PaymentGateway { get; set; }
    }
}