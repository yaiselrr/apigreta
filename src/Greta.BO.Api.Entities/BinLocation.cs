using System;
using System.Collections.Generic;
using Greta.BO.Api.Entities.Enum;
using Greta.Sdk.EFCore.Interfaces;

namespace Greta.BO.Api.Entities
{
    public class BinLocation : BaseEntityLong
    {
        public string Name { get; set; }

        public int Aisle { get; set; }
        
        public int Side { get; set; }

        public int Section { get; set; }
        
        public int Shelf { get; set; }

        public long Store { get; set; }

        public virtual List<StoreProduct> StoreProducts { get; set; }
    }
}