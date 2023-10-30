using Greta.BO.Api.Entities.Enum;
using Greta.Sdk.EFCore.Interfaces;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Greta.BO.Api.Entities
{
    public class ScaleReasonCodes : BaseEntityLong, IFullSyncronizable
    {
        
        public string Name { get; set; }

        //public long DepartmentId { get; set; }

        //public Department Department { get; set; }

        public ScaleReasonCodesType Type { get; set; }

    }
}
