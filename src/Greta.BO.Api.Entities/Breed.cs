using System.Collections.Generic;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.Api.Entities.Interfaces;

namespace Greta.BO.Api.Entities
{
    public class Breed : BaseEntityLong, INameUniqueEntity
    {
        public string Name { get; set; }
        public AnimalBreedType AnimalBreedType { get; set; }
        public int Maxx { get; set; }
        public virtual List<Animal> Animals { get; set; }
        public virtual List<Scalendar> Scalendars { get; set; }
    }
}