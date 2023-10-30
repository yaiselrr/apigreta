using System.Collections.Generic;
using Greta.BO.Api.Entities.Attributes;
using Greta.BO.Api.Entities.Interfaces;
using Greta.Sdk.EFCore.Interfaces;

namespace Greta.BO.Api.Entities
{
    public class Department : BaseEntityLong, IFullSyncronizable, INameUniqueEntity
    {
        [FieldInfo("DEP_ID", 2)]
        public int DepartmentId { get; set; }
        [FieldInfo("DEP_NAME", 1, ValueFormating.EscapeQuotes)]
        public string Name { get; set; }
        [FieldInfo("PERISHABLE", 3, ValueFormating.BooleanToNumber)]
        public bool Perishable { get; set; }
        
        public virtual List<Discount> Discounts { get; set; }
        public virtual List<Category> Categories { get; set; }
        public virtual List<Product> Products { get; set; }
        [FieldInfo("BCOLOR", 4)]
        public string BackgroundColor { get; set; }
        [FieldInfo("FCOLOR", 5)]
        public string ForegroundColor { get; set; }

        #region Store

        // public long StoreId { get; set; }
        // public Store Store { get; set; }

        #endregion Store

        public virtual List<ExternalScale> ExternalScales { get; set; }
        public virtual List<OnlineStore> OnlineStores { get; set; }
    }
}