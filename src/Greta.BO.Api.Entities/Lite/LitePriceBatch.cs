using System;
using Greta.BO.Api.Entities.Attributes;
using Greta.BO.Api.Entities.Enum;

namespace Greta.BO.Api.Entities.Lite
{
    [SqlTable("Batch")]
    public class LitePriceBatch : BaseEntityLong
    {
        public string Name { get; set; }

        public DateTime StartTime { get; set; }

        public RetailPriceBatchType Type { get; set; }

        public static LitePriceBatch Convert(PriceBatch from)
        {
            return new()
            {
                Id = from.Id,
                State = from.State,
                UserCreatorId = from.UserCreatorId,
                CreatedAt = from.CreatedAt,
                UpdatedAt = from.UpdatedAt,

                Name = from.Name,
                StartTime = from.StartTime,
                Type = from.Type,
            };
        }
    }
}