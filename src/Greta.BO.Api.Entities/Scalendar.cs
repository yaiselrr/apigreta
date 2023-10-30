using System.Collections.Generic;
using Greta.Sdk.EFCore.Interfaces;

namespace Greta.BO.Api.Entities
{
    public class Scalendar : BaseEntityLong
    {
        public int DayId { get; set; }
        public string Day { get; set; }

        public virtual List<Breed> Breeds { get; set; }
    }
}