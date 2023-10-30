using Greta.BO.Api.Entities.Enum;

namespace Greta.BO.Api.Entities
{
    public class SaleTax : BaseEntityLong
    {
        public long? SaleId { get; set; }
        public Sale Sale { get; set; }
        public long? SaleProductId { get; set; }
        public SaleProduct SaleProduct { get; set; }

        public string Name { get; set; }
        public TaxType Type { get; set; }
        /// <summary>
        /// Amount of this tax on sale
        /// </summary>
        public decimal Amount { get; set; }

        public decimal Value { get; set; }
        
        public bool UseSpecialTax { get; set; }
    }

}
