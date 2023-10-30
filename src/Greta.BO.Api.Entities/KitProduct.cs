using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Greta.BO.Api.Entities
{
    [Table("KitProduct")]
    public class KitProduct : Product
    {
        public List<Product> Products { get; set; }
    }
}