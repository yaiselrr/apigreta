using Greta.BO.Api.Entities.Attributes;

namespace Greta.BO.Api.Entities.Lite
{
    [SqlTable("ScaleLabelDefinition")]
    public class LiteScaleLabelDefinition : BaseEntityLong
    {
        public long ScaleProductId { get; set; }
        public long ScaleLabelType1Id { get; set; }
        public long? ScaleLabelType2Id { get; set; }
        public long ScaleBrandId { get; set; }

        public static LiteScaleLabelDefinition Convert(ScaleLabelDefinition from)
        {
            return new()
            {
                Id = from.Id,
                State = from.State,
                UserCreatorId = from.UserCreatorId,
                CreatedAt = from.CreatedAt,
                UpdatedAt = from.UpdatedAt,
                ScaleProductId = from.ScaleProductId,
                ScaleLabelType1Id = from.ScaleLabelType1Id,
                ScaleLabelType2Id = from.ScaleLabelType2Id,
                ScaleBrandId = from.ScaleBrandId
            };
        }
    }
}