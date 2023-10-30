
using System.ComponentModel.DataAnnotations;

namespace Greta.BO.BusinessLogic.Models.Dto.Search
{
    public class AllProductsByStoreRequestModel: BaseSearchModel
    {
        [Required]
        public long StoreId { get; set; }
        public string UPC { get; set; }
        public string Name { get; set; }
    }
}
