using Greta.BO.Api.Entities.Attributes;

namespace Greta.BO.Api.Entities.Lite
{
    [SqlTable("PriceBatchDetail")]
    public class LitePriceBatchDetail : BaseEntityLong
    {
        public decimal Price { get; set; }
        public long? ProductId { get; set; }
        public long? FamilyId { get; set; }
        public long? CategoryId { get; set; }

        public long HeaderId { get; set; }

        public static LitePriceBatchDetail Convert(PriceBatchDetail from)
        {
            return new() {

                Id = from.Id,
                State = from.State,
                UserCreatorId = from.UserCreatorId,
                CreatedAt = from.CreatedAt,
                UpdatedAt = from.UpdatedAt,

                Price = from.Price,
                ProductId = from.ProductId,
                FamilyId = from.FamilyId,
                CategoryId = from.CategoryId,
                HeaderId = from.HeaderId

            };
        }
    }
}
