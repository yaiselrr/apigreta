using System.Collections.Generic;
using Greta.Sdk.EFCore.Interfaces;

namespace Greta.BO.Api.Entities
{
    public class ScaleHomeFav : BaseEntityLong, IFullSyncronizable
    {
        public long DepartmentId { get; set; }

        public long StoreId { get; set; }

        public Department Department { get; set; }

        public Store Store { get; set; }

        public virtual List<ScaleProduct> ScaleProducts { get; set; }
    }
}