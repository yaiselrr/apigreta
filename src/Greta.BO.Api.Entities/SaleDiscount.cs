using Greta.BO.Api.Entities.Enum;

namespace Greta.BO.Api.Entities
{
    public class SaleDiscount: BaseEntityLong
    {
        public long? SaleId { get; set; }
        public Sale Sale { get; set; }

        public string Name { get; set; }
        public DiscountType Type { get; set; }
        /// <summary>
        /// Amount of this tax on sale
        /// </summary>
        public decimal Amount { get; set; }

        public decimal Value { get; set; }
    }

}
