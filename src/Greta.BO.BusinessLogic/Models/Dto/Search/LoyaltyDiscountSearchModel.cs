using System.ComponentModel.DataAnnotations;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;

namespace Greta.BO.BusinessLogic.Models.Dto.Search
{
    public class LoyaltyDiscountSearchModel : BaseSearchModel, IMapFrom<LoyaltyDiscount>
    {
        public string Name { get; set; }
    }
}