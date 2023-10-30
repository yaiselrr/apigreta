using System.Collections.Generic;

namespace Greta.BO.Api.Entities
{
    public class Rancher : BaseEntityLong
    {
        public string Name { get; set; }
        public virtual List<Animal> Animals { get; set; }
    }
}