using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;

namespace Greta.BO.BusinessLogic.Models.Dto.Search
{
    public class TaxSearchModel : BaseSearchModel, IMapFrom<Tax>
    {
        public string Name { get; set; }

        public string Description { get; set; }
    }
}