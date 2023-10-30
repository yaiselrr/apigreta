using Greta.BO.Api.Entities;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;
using Greta.Sdk.Core.Abstractions;

using System;
using System.ComponentModel.DataAnnotations;

namespace Greta.BO.BusinessLogic.Models.Dto
{
    public class ScaleReasonCodesModel : IDtoLong<string>, IMapFrom<ScaleReasonCodes>
    {
        [Required]
        [StringLength(64, ErrorMessage = "The {0} field not is valid")]
        public string Name { get; set; }

        //[Required] public long DepartmentId { get; set; }

        //public DepartmentModel Department { get; set; }

        public ScaleReasonCodesType Type { get; set; }

        public bool State { get; set; }
        public string UserCreatorId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public long Id { get; set; }

    }
}
