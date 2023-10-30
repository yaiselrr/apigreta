using Greta.BO.Api.Entities.Interfaces;

namespace Greta.BO.Api.Entities
{
    public class Grind : BaseEntityLong, INameUniqueEntity
    {
        public string Name { get; set; }
    }
}