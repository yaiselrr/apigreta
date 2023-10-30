using Greta.BO.Api.Entities.Attributes;

namespace Greta.BO.Api.Entities.Lite
{
    [SqlTable("ScaleCategory")]
    public class LiteScaleCategory : BaseEntityLong
    {
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public long DepartmentId { get; set; }
        public string BackgroundColor { get; set; }
        public string ForegroundColor { get; set; }
        public long? ParentId { get; set; }

        public static LiteScaleCategory Convert(ScaleCategory from)
        {
            return new()
            {
                Id = from.Id,
                State = from.State,
                UserCreatorId = from.UserCreatorId,
                CreatedAt = from.CreatedAt,
                UpdatedAt = from.UpdatedAt,

                Name = from.Name,
                CategoryId = from.CategoryId,
                DepartmentId = from.DepartmentId,
                BackgroundColor = from.BackgroundColor,
                ForegroundColor = from.ForegroundColor,
                ParentId = from.ParentId
            };
        }
    }
}