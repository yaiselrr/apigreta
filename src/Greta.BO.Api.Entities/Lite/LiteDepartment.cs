using Greta.BO.Api.Entities.Attributes;

namespace Greta.BO.Api.Entities.Lite
{
    [SqlTable("Department")]
    public class LiteDepartment : BaseEntityLong
    {
        [SqlColumn("DepartmentId")]
        public int DepartmentId { get; set; }
        public string Name { get; set; }
        public bool Perishable { get; set; }
        public string BackgroundColor { get; set; }
        public string ForegroundColor { get; set; }

        public static LiteDepartment Convert(Department from)
        {
            return new LiteDepartment
            {
                Id = from.Id,
                State = from.State,
                UserCreatorId = from.UserCreatorId,
                CreatedAt = from.CreatedAt,
                UpdatedAt = from.UpdatedAt,

                DepartmentId = from.DepartmentId,
                Name = from.Name,
                Perishable = from.Perishable,
                BackgroundColor = from.BackgroundColor,
                ForegroundColor = from.ForegroundColor
            };
        }
    }
}