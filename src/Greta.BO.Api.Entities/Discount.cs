using System;
using System.Collections.Generic;
using Greta.BO.Api.Entities.Enum;
using Greta.BO.Api.Entities.Interfaces;
using Greta.Sdk.EFCore.Interfaces;

namespace Greta.BO.Api.Entities
{
    public class Discount : BaseEntityLong, IFullSyncronizable, INameUniqueEntity
    {
        public string Name { get; set; }
        public DiscountType Type { get; set; }
        public decimal Value { get; set; }
        public bool NotAllowAnyOtherDiscount { get; set; }
        public bool ApplyToTotalSale { get; set; }
        public bool PromptForPrice { get; set; }
        public bool ApplyToProduct { get; set; }
        public bool ApplyAutomatically { get; set; }
        public bool ApplyToCustomerOnly { get; set; }
        public bool ActiveOnPeriod { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? Monday { get; set; }
        public bool? Tuesday { get; set; }
        public bool? Wednesday { get; set; }
        public bool? Thursday { get; set; }
        public bool? Friday { get; set; }
        public bool? Saturday { get; set; }
        public bool? Sunday { get; set; }

        public long? DepartmentId { get; set; }
        public long? CategoryId { get; set; }

        public Department Department { get; set; }
        public Category Category { get; set; }
        public virtual List<Product> Products { get; set; }
        
        public virtual List<Customer> Customers { get; set; }

        // public List<StoreProduct> StoreProducts { get; set; }
        // public List<Family> Families { get; set; }
    }
}