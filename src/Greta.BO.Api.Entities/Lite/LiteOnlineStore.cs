using System.Collections.Generic;
using System.Linq;
using Greta.BO.Api.Entities.Attributes;
using Greta.BO.Api.Entities.Enum;

namespace Greta.BO.Api.Entities.Lite
{
    [SqlTable("OnlineStore")]
    public class LiteOnlineStore : BaseEntityLong
    {        

        public string Name { get; set; }
        public string NameWebsite { get; set; }
        public StoreType Type { get; set; }
        public LocationServerType LocationServerType { get; set; }
        public bool IsActiveWebSite { get; set; }
        public bool IsStockUpdated { get; set; }
        public bool IsAllowStorePickup { get; set; }        
        public bool IsAssociated { get; set; }        
        public string RefreshToken { get; set; }
        public long StoreId { get; set; }
        public static LiteOnlineStore Convert(OnlineStore from)
        {
            return new()
            {
                Id = from.Id,
                State = from.State,
                UserCreatorId = from.UserCreatorId,
                CreatedAt = from.CreatedAt,
                UpdatedAt = from.UpdatedAt,
                Name = from.Name,
                NameWebsite = from.Name,
                Type = from.Type,
                LocationServerType = from.LocationServerType,
                IsActiveWebSite = from.IsActiveWebSite,
                IsAssociated = from.IsAssociated,
                IsStockUpdated = from.IsStockUpdated,
                IsAllowStorePickup = from.IsAllowStorePickup,
                RefreshToken = from.RefreshToken,
                StoreId = from.StoreId,
            };
        }   
    }
}