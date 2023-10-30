using System;
using System.Collections.Generic;
using System.Linq;
using Greta.BO.Api.Entities.Attributes;
using Greta.BO.Api.Entities.Enum;

namespace Greta.BO.Api.Entities.Lite
{
    [SqlTable("MixAndMatch")]
    public class LiteMixAndMatch : BaseEntityLong
    {
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public int QTY { get; set; }
        public MixAndMatchType MixAndMatchType { get; set; }
        public bool ActivePeriod { get; set; }
        
        public bool NotAllowAnyOtherDiscount { get; set; }
        public bool ApplyToCustomerOnly { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public long? ProductBuyId { get; set; }
        [SqlFkTable("FamilyMixAndMatch", "MixAndMatchsId")]
        [SqlFkColumn("FamiliesId")]
        public List<long> Families { get; set; }
        [SqlFkTable("MixAndMatchProduct", "MixAndMatchsId")]
        [SqlFkColumn("ProductsId")]
        public List<long> Products { get; set; }

        public static LiteMixAndMatch Convert(MixAndMatch from)
        {
            return new()
            {
                Id = from.Id,
                State = from.State,
                UserCreatorId = from.UserCreatorId,
                CreatedAt = from.CreatedAt,
                UpdatedAt = from.UpdatedAt,

                Name = from.Name,
                Amount = from.Amount,
                QTY = from.QTY,
                MixAndMatchType = from.MixAndMatchType,
                ActivePeriod = from.ActivePeriod,
                NotAllowAnyOtherDiscount = from.NotAllowAnyOtherDiscount,
                ApplyToCustomerOnly = from.ApplyToCustomerOnly,
                StartDate = from.StartDate,
                EndDate = from.EndDate,
                ProductBuyId = from.ProductBuyId,
                Families = from.Families?.Select(x => x.Id).ToList(),
                Products = from.Products?.Select(x => x.Id).ToList()
            };
        }
    }
}