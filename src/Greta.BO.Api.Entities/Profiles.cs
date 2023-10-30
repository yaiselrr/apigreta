using Greta.BO.Api.Entities.Interfaces;
using Greta.Sdk.EFCore.Interfaces;
using System.Collections.Generic;

namespace Greta.BO.Api.Entities
{
    public class Profiles : BaseEntityLong, IFullSyncronizable, INameUniqueEntity
    {
        public string Name { get; set; }

        public long ApplicationId { get; set; }

        public virtual ClientApplication Application { get; set; }
        public virtual List<Permission> Permissions { get; set; }

        public virtual List<FunctionGroup> FunctionGroups { get; set; }
    }
}