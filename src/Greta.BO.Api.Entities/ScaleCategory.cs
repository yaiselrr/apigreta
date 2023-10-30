using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Greta.BO.Api.Entities.Interfaces;
using Greta.Sdk.EFCore.Interfaces;

namespace Greta.BO.Api.Entities
{
    public class ScaleCategory : BaseEntityLong, IFullSyncronizable, INameUniqueEntity
    {
        [Required]
        [StringLength(64, ErrorMessage = "The {0} field not is valid")]
        public string Name { get; set; }

        public int CategoryId { get; set; }

        public long DepartmentId { get; set; }

        public Department Department { get; set; }
        
        public long? ParentId { get; set; }

        public ScaleCategory Parent { get; set; }
        public virtual List<ScaleCategory> Children { get; set; }

        public string BackgroundColor { get; set; }
        public string ForegroundColor { get; set; }
    }
}