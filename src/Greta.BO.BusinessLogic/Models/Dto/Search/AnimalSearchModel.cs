using System;
using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;

namespace Greta.BO.BusinessLogic.Models.Dto.Search
{
    public class AnimalSearchModel : BaseSearchModel, IMapFrom<Animal>
    {
        public long  StoreId { get; set; }
        public string Tag { get; set; }

        public long RancherId { get; set; }

        public long BreedId { get; set; }
        public DateTime? DateReceived { get; set; }
        public DateTime? DateSlaughtered { get; set; }
    }
}