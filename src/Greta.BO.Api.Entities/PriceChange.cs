using System;

namespace Greta.BO.Api.Entities
{
    internal class PriceChange : BaseEntityLong
    {
        public long ProductId { get; set; }
        public decimal Price { get; set; }
        public DateTime from { get; set; }
        public DateTime to { get; set; }
    }
}