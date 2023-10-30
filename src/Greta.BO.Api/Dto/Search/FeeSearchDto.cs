using System.ComponentModel.DataAnnotations;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;
using Greta.BO.BusinessLogic.Models.Dto.Search;

namespace Greta.BO.Api.Dto.Search
{
    public class FeeSearchDto : BaseSearchDto, IMapFrom<FeeSearchModel>
    {
        public string Name { get; set; }
        public string Description { get; set; }

        [Required] public DiscountType Type { get; set; }
    }
}