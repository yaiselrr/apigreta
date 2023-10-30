using Greta.BO.Api.Entities;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;

namespace Greta.BO.BusinessLogic.Models.Dto.Search
{
    public class DiscountSearchModel : BaseSearchModel, IMapFrom<Discount>
    {
        public string Name { get; set; }

        public DiscountType Type { get; set; }
    }
}