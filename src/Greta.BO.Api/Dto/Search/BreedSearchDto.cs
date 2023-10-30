
using Greta.BO.Api.Entities.Enum;

using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;
using Greta.BO.BusinessLogic.Models.Dto.Search;

namespace Greta.BO.Api.Dto.Search
{
    public class BreedSearchDto : BaseSearchDto, IMapFrom<BreedSearchModel>
    {
        public string Name { get; set; }

        public AnimalBreedType AnimalBreedType { get; set; }

    }
}