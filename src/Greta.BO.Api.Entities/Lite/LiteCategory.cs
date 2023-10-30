using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Greta.BO.Api.Entities.Attributes;

namespace Greta.BO.Api.Entities.Lite
{
    [SqlTable("Category")]
    public class LiteCategory : BaseEntityLong
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long DepartmentId { get; set; }
        public bool VisibleOnPos { get; set; }
        public bool AddOnlineStore { get; set; }

        public string BackgroundColor { get; set; }
        public string ForegroundColor { get; set; }

        [SqlFkTable("CategoryTax", "CategoriesId")]
        [SqlFkColumn("TaxId")]
        public virtual List<long> Taxes { get; set; }

        public static LiteCategory Convert(Category from, List<long> storeTaxes)
        {
            return new()
            {
                Id = from.Id,
                State = from.State,
                UserCreatorId = from.UserCreatorId,
                CreatedAt = from.CreatedAt,
                UpdatedAt = from.UpdatedAt,
                VisibleOnPos = from.VisibleOnPos,

                CategoryId = from.CategoryId,
                Name = from.Name,
                AddOnlineStore = from.AddOnlineStore,
                Description = from.Description,
                DepartmentId = from.DepartmentId,
                BackgroundColor = from.BackgroundColor,
                ForegroundColor = from.ForegroundColor,
                Taxes = from.Taxs?.Select(x => x.Id).Where(storeTaxes.Contains).ToList()

            };
        }
    }
}