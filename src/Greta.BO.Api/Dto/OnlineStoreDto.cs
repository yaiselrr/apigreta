using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AutoMapper;
using Greta.BO.Api.Entities;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.BusinessLogic.Core.Startup.AutoMapper;
using Greta.BO.BusinessLogic.Models.Dto;
using Greta.Sdk.Core.Abstractions;

namespace Greta.BO.Api.Dto
{
    public class OnlineStoreDto : IDtoLong<string>, IMapFrom<OnlineStoreModel>
    {
        [Required]
        [StringLength(64, ErrorMessage = "The {0} field not is valid")]
        public string Name { get; set; }
        public string NameWebsite { get; set; }
        public LocationServerType LocationServerType { get; set; }
        public bool IsActiveWebSite { get; set; }
        public bool IsStockUpdated { get; set; }
        public bool IsAllowStorePickup { get; set; }
        [Required] public long StoreId { get; set; }
        public bool IsAssociated { get; set; }

        public long Id { get; set; }
        public bool State { get; set; }
        public string UserCreatorId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}