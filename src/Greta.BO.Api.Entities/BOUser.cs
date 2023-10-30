using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Greta.Sdk.EFCore.Interfaces;

namespace Greta.BO.Api.Entities
{
    public class BOUser : BaseEntityLong
    {
        // private long? boProfileId;
        // private long? posProfileId;

        public string FirstName { get; set; }
        public string LastName { get; set; }
        
        public long? RoleId { get; set; }
        public Role Role { get; set; }

        public long? BOProfileId{ get; set; }
        // {
        //     get => boProfileId == -1 ? null : boProfileId;
        //     set => boProfileId = value;
        // }

        public Profiles BOProfile { get; set; }

        public long? POSProfileId{ get; set; }
        // {
        //     get => posProfileId == -1 ? null : posProfileId;
        //     set => posProfileId = value;
        // }
        
        public List<Store> Stores { get; set; }

        public Profiles POSProfile { get; set; }
        public string UserId { get; set; }

        public string UserName { get; set; }
        
        public string Pin { get; set; }

        public string Email { get; set; }
        public DateTime? Expire { get; set; }

        [NotMapped] public string PhoneNumber { get; set; }
        public virtual List<VendorOrder> VendorOrders { get; set; }
    }
}