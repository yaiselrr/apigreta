using Greta.BO.Api.Entities.Attributes;

namespace Greta.BO.Api.Entities.Lite
{
    [SqlTable("Vendor")]
    public class LiteVendor : BaseEntityLong
    {
        public string Name { get; set; }

        public static LiteVendor Convert(Vendor from)
        {
            return new LiteVendor
            {
                Id = from.Id,
                State = from.State,
                UserCreatorId = from.UserCreatorId,
                CreatedAt = from.CreatedAt,
                UpdatedAt = from.UpdatedAt,
                Name = from.Name
            };
        }
    }
}
