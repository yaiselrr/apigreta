using Greta.BO.Api.Entities.Attributes;
using Greta.BO.Api.Entities.Enum;

namespace Greta.BO.Api.Entities.Lite
{
    [SqlTable("Tax")]
    public class LiteTax : BaseEntityLong
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public TaxType Type { get; set; }
        public double Value { get; set; }
        public double? SpecialValue { get; set; }

        public static LiteTax Convert(Tax from)
        {
            return new()
            {
                Id = from.Id,
                State = from.State,
                UserCreatorId = from.UserCreatorId,
                CreatedAt = from.CreatedAt,
                UpdatedAt = from.UpdatedAt,
                Name = from.Name,
                Description = from.Description,
                Type = from.Type,
                Value = from.Value,
                SpecialValue = from.SpecialValue
            };
        }
    }
}