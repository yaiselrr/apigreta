using System.Collections.Generic;
using Greta.BO.Api.Entities.Interfaces;
using Greta.Sdk.EFCore.Interfaces;

namespace Greta.BO.Api.Entities
{
    public class CutListTemplate : BaseEntityLong, INameUniqueEntity
    {
        public string Name { get; set; }

        public List<ScaleProduct> ScaleProducts { get; set; }       
    }
}