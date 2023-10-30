using System;
using System.Collections.Generic;
using Greta.Sdk.EFCore.Interfaces;

namespace Greta.BO.Api.Entities
{
    [Obsolete]
    public class ScaleBrand : BaseEntityLong, IFullSyncronizable
    {
        public string Name { get; set; }

        public string Manufacture { get; set; }

        public virtual List<ExternalScale> ExternalScales { get; set; }
        // public virtual List<ScaleLabelDefinition> ScaleLabelDefinitions { get; set; }
    }
}