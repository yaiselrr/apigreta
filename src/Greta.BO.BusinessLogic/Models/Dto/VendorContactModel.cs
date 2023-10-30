using System;
using System.ComponentModel.DataAnnotations;
using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;
using Greta.Sdk.Core.Abstractions;

namespace Greta.BO.BusinessLogic.Models.Dto
{
    public class VendorContactModel : IDtoLong<string>, IMapFrom<VendorContact>
    {
        [Required]
        [StringLength(64, ErrorMessage = "The {0} field not is valid")]
        public string Contact { get; set; }

        [Required]
        [Phone]
        [StringLength(12, ErrorMessage = "The {0} field not is valid")]
        public string Phone { get; set; }

        [Required] [EmailAddress] public string Email { get; set; }

        public ImageModel Image { get; set; }
        
        public bool Primary { get; set; }

        public long VendorId { get; set; }
        public string Fax { get; set; }
        public long Id { get; set; }
        public bool State { get; set; }
        public string UserCreatorId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}