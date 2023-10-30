using System;
using System.Collections.Generic;
using System.Linq;
using Greta.BO.Api.Entities.Attributes;
using Greta.BO.Api.Entities.Enum;

namespace Greta.BO.Api.Entities.Lite
{
    [SqlTable("Discount")]
    public class LiteDiscount : BaseEntityLong
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

        [SqlFkTable("DiscountProduct", "DiscountsId")]
        [SqlFkColumn("ProductsId")]
        public virtual List<long> Products { get; set; }

        public static LiteDiscount Convert(Discount from)
        {
            return new()
            {
                Id = from.Id,
                State = from.State,
                UserCreatorId = from.UserCreatorId,
                CreatedAt = from.CreatedAt,
                UpdatedAt = from.UpdatedAt,

                Name = from.Name,
                Type = from.Type,
                Value = from.Value,
                PromptForPrice = from.PromptForPrice,
                
                NotAllowAnyOtherDiscount = from.NotAllowAnyOtherDiscount,
                ApplyToTotalSale = from.ApplyToTotalSale,
                ApplyToProduct = from.ApplyToProduct,
                ApplyAutomatically = from.ApplyAutomatically,
                ApplyToCustomerOnly = from.ApplyToCustomerOnly,
                ActiveOnPeriod = from.ActiveOnPeriod,
                StartDate = from.StartDate,
                EndDate = from.EndDate,
                Monday = from.Monday,
                Tuesday = from.Tuesday,
                Wednesday = from.Wednesday,
                Thursday = from.Thursday,
                Friday = from.Friday,
                Saturday = from.Saturday,
                Sunday = from.Sunday,
                DepartmentId = from.DepartmentId,
                CategoryId = from.CategoryId,
                Products = from.Products?.Select(x => x.Id).ToList()
            };
        }
    }
}