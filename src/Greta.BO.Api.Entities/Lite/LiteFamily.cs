using Greta.BO.Api.Entities.Attributes;

namespace Greta.BO.Api.Entities.Lite
{
    [SqlTable("Family")]
    public class LiteFamily : BaseEntityLong
    {
        public string Name { get; set; }

        public static LiteFamily Convert(Family from)
        {
            return new()
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