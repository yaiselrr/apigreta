using Greta.BO.Api.Entities.Attributes;
using Greta.BO.Api.Entities.Enum;

namespace Greta.BO.Api.Entities.Lite
{
    [SqlTable("ScaleLabelType")]
    public class LiteScaleLabelType : BaseEntityLong
    {
        public string Name { get; set; }
        public int LabelId { get; set; }
        public ScaleType ScaleType { get; set; }
        public string Design { get; set; }

        public static LiteScaleLabelType Convert(ScaleLabelType from)
        {
            return new()
            {
                Id = from.Id,
                State = from.State,
                UserCreatorId = from.UserCreatorId,
                CreatedAt = from.CreatedAt,
                UpdatedAt = from.UpdatedAt,
                Name = from.Name,
                LabelId = from.LabelId,
                ScaleType = from.ScaleType,
                Design = from.Design//from.ScaleLabelDesign?.Design
            };
        }
    }
}