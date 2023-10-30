using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Greta.BO.Api.Entities.Attributes;

namespace Greta.BO.Api.Entities.Lite
{
    [SqlTable("Customer")]
    public class LiteCustomer : BaseEntityLong
    {
        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public bool TaxExcept { get; set; }
        
        public bool UsePrice2 { get; set; }
        public string TaxID { get; set; }

        public int StoreCredit { get; set; }
        
        // [NotMapped]
        // [SqlColumn("DiscountMaximum")]
        // public decimal DiscountMaximum { get; set; }
        // [NotMapped]
        // public decimal DiscountValue { get; set; }
        [SqlFkTable("CustomerMixAndMatch", "CustomerId")]
        [SqlFkColumn("MixAndMatchId")]
        public List<long> MixAndMatches { get; set; }
        
        [SqlFkTable("CustomerDiscount", "CustomerId")]
        [SqlFkColumn("DiscountId")]
        public List<long> Discounts { get; set; }
        // public DateTime LastBuy { get; set; }
         
        public static LiteCustomer Convert(Customer from)
        {
            return new LiteCustomer()
            {
                Id = from.Id,
                State = from.State,
                UserCreatorId = from.UserCreatorId,
                CreatedAt = from.CreatedAt,
                UpdatedAt = from.UpdatedAt,
                UsePrice2 = from.UsePrice2,
                LastName = from.LastName,
                FirstName = from.FirstName,
                Phone = from.Phone,
                Email = from.Email,
                TaxExcept = from.TaxExcept,
                TaxID = from.TaxID,
                StoreCredit = from.StoreCredit,
                // DiscountMaximum =  from.DiscountMaximum,
                // DiscountValue =  from.DiscountValue,
                MixAndMatches = from.MixAndMatches?.Select(x => x.Id).ToList(),
                Discounts = from.Discounts?.Select(x => x.Id).ToList(),
                // LastBuy =  from.LastBuy,
            };
        }
    }
}