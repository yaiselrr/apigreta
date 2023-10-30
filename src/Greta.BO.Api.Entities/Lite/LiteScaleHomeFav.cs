using System.Collections.Generic;
using System.Linq;
using Greta.BO.Api.Entities.Attributes;

namespace Greta.BO.Api.Entities.Lite
{
    [SqlTable("ScaleHomeFav")]
    public class LiteScaleHomeFav : BaseEntityLong
    {
        public long DepartmentId { get; set; }
        [SqlFkTable("ScaleHomeFavScaleProduct", "ScaleHomeFavsId")]
        [SqlFkColumn("ScaleProductsId")]
        public virtual List<long> ScaleProducts { get; set; }

        public static LiteScaleHomeFav Convert(ScaleHomeFav from)
        {
            return new()
            {
                Id = from.Id,
                State = from.State,
                UserCreatorId = from.UserCreatorId,
                CreatedAt = from.CreatedAt,
                UpdatedAt = from.UpdatedAt,
                DepartmentId = from.DepartmentId,
                ScaleProducts = from.ScaleProducts?.Select(x => x.Id).ToList()
            };
        }
    }
}