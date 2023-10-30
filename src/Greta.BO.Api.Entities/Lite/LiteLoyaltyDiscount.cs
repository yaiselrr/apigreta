using Greta.BO.Api.Entities.Attributes;

namespace Greta.BO.Api.Entities.Lite
{
    [SqlTable("LoyaltyDiscount")]
    public class LiteLoyaltyDiscount : BaseEntityLong
    {
        public string Name { get; set; }
        public decimal Value { get; set; }
        public decimal Maximum { get; set; }
        
        public static LiteLoyaltyDiscount Convert(LoyaltyDiscount from)
        {
            if (from == null) return null;
            return new()
            {
                Id = from.Id,
                State = from.State,
                UserCreatorId = from.UserCreatorId,
                CreatedAt = from.CreatedAt,
                UpdatedAt = from.UpdatedAt,
                Name = from.Name,
                
                Value = from.Value,
                Maximum = from.Maximum
            };
        }
    }
}