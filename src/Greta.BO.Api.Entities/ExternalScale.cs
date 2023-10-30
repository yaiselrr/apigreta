using System;
using System.Collections.Generic;
using Greta.BO.Api.Entities.Enum;
using Greta.Sdk.EFCore.Interfaces;

namespace Greta.BO.Api.Entities
{
    public class ExternalScale : BaseEntityLong, IFullSyncronizable
    {
        public string Ip { get; set; }

        public string Port { get; set; }

        public BoExternalScaleType ExternalScaleType { get; set; }

        public long StoreId { get; set; }

        public Store Store { get; set; }

        public List<Department> Departments { get; set; }

        /// <summary>
        /// Store the last date the scale report finish a Department update
        /// </summary>
        public DateTime LastDepartmentUpdate { get; set; }
        /// <summary>
        /// Store the last date the scale report finish a Category update
        /// </summary>
        public DateTime LastCategoryUpdate { get; set; }
        /// <summary>
        /// Store the last date the scale report finish a Plu update
        /// </summary>
        public DateTime LastPluUpdate { get; set; }

        public long? SyncDeviceId { get; set; }
        public Device SyncDevice { get; set; }
    }
}