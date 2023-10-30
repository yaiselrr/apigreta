using Greta.BO.Api.Entities;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;

namespace Greta.BO.BusinessLogic.Models.Dto.Search
{
    public class BreedSearchModel : BaseSearchModel, IMapFrom<Breed>
    {
        public string Name { get; set; }

        public AnimalBreedType AnimalBreedType { get; set; }

    }
}