using Greta.BO.Api.Entities.Interfaces;
using Greta.Sdk.EFCore.Interfaces;
using System.Collections.Generic;

namespace Greta.BO.Api.Entities
{
    public class Role : BaseEntityLong, IFullSyncronizable, INameUniqueEntity
    {
        public string Name { get; set; }

        public bool AllStores { get; set; }

        public long? RegionId { get; set; }

        public Region Region { get; set; }

        public List<Store> Stores { get; set; }
    }
}