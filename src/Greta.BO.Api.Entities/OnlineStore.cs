using System.Collections.Generic;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.Api.Entities.Interfaces;

namespace Greta.BO.Api.Entities
{
    public class OnlineStore : BaseEntityLong, INameUniqueEntity
    {
        public string Name { get; set; }
        public string NameWebsite { get; set; }
        public StoreType Type { get; set; }
        public LocationServerType LocationServerType { get; set; }
        /// <summary>
        /// IsActiveWebSite Check box “you can choose if the site is active or no”
        /// </summary>
        public bool IsActiveWebSite { get; set; }
        /// <summary>
        /// IsStockUpdated Check box “you can choose  if the stock is updated”
        /// </summary>
        public bool IsStockUpdated { get; set; }
        /// <summary>
        /// IsAllowStorePickup Check box “you can choose  if the site allows store pickup”
        /// </summary>
        public bool IsAllowStorePickup { get; set; }
        public bool IsAssociated { get; set; }
        public bool Isdeleted { get; set; }        
        public string RefreshToken { get; set; }
        public string Instance { get; set; }
        public string Url { get; set; }
        public string CustomDns { get; set; }
        public long StoreId { get; set; }
        public Store Store { get; set; }     
        public virtual List<Department> Departments { get; set; }   
        
        public virtual List<OnlineCategory> OnlineCategories { get; set; }
        
        public virtual List<OnlineProduct> OnlineProducts { get; set; }
        
    }
}