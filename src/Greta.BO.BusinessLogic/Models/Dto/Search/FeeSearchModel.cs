using System.ComponentModel.DataAnnotations;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;

namespace Greta.BO.BusinessLogic.Models.Dto.Search
{
    public class FeeSearchModel : BaseSearchModel, IMapFrom<Fee>
    {
        public string Name { get; set; }
        public string Description { get; set; }

        [Required] public DiscountType Type { get; set; }
    }
}