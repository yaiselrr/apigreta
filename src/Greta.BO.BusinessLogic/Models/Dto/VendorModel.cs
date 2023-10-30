using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Greta.BO.Api.Entities;
using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;
using Greta.Sdk.Core.Abstractions;

namespace Greta.BO.BusinessLogic.Models.Dto
{
    public class VendorModel : IDtoLong<string>, IMapFrom<Vendor>
    {
        [Required]
        [StringLength(40, ErrorMessage = "The {0} field not is valid")]
        public string Name { get; set; }

        [Required] public string AccountNumber { get; set; }

        public string Note { get; set; }

        [Required] public double MinimalOrder { get; set; }

        public List<VendorContactModel> VendorContacts { get; set; }

        [Required] public string Address1 { get; set; }

        public string Address2 { get; set; }
        public string CityName { get; set; }
        public string ProvinceName { get; set; }
        public string CountryName { get; set; }

        //public long CityId { get; set; }

        public long ProvinceId { get; set; }

        public long CountryId { get; set; }

        [Required] public string Zip { get; set; }

        public long Id { get; set; }

        public bool State { get; set; }
        public string UserCreatorId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}