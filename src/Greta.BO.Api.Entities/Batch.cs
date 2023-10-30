using System;
using System.Collections.Generic;
using Greta.BO.Api.Entities.Enum;
using Greta.Sdk.EFCore.Interfaces;

namespace Greta.BO.Api.Entities
{
    public class Batch : BaseEntityLong, IFullSyncronizable
    {
        public string Name { get; set; }

        public DateTime StartTime { get; set; }

        //public bool Applied { get; set; }

        public virtual List<Store> Stores { get; set; }

        public RetailPriceBatchType Type { get; set; }
        public virtual List<PriceBatchDetail> PriceBatchDetails { get; set; }
    }
}