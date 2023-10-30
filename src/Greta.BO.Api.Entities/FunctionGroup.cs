using System.Collections.Generic;

namespace Greta.BO.Api.Entities
{
    public class FunctionGroup : BaseEntityLong
    {
        public string Name { get; set; }

        public long ClientApplicationId { get; set; }

        public virtual ClientApplication ClientApplication { get; set; }

        public virtual List<Permission> Permissions { get; set; }
    }
}