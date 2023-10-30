using System.Collections.Generic;

namespace Greta.BO.Api.Entities
{
    public class ClientApplication : BaseEntityLong
    {
        public string Name { get; set; }

        //Here need more information  about the application

        public virtual List<FunctionGroup> FunctionGroups { get; set; }
    }
}