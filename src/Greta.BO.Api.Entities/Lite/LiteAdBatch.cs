using Greta.BO.Api.Entities.Enum;
using System;
using Greta.BO.Api.Entities.Attributes;

namespace Greta.BO.Api.Entities.Lite
{
    [SqlTable("Batch")]
    public class LiteAdBatch : BaseEntityLong
    {
        public string Name { get; set; }

        public DateTime StartTime { get; set; }

        public RetailPriceBatchType Type { get; set; }

        public DateTime EndTime { get; set; }
        public static LiteAdBatch Convert(AdBatch from)
        {
            return new() {
                Id = from.Id,
                State = from.State,
                UserCreatorId = from.UserCreatorId,
                CreatedAt = from.CreatedAt,
                UpdatedAt = from.UpdatedAt,

                Name = from.Name,
                StartTime = from.StartTime,
                EndTime = from.EndTime,
                Type = from.Type,
            };
        }
    }
}
