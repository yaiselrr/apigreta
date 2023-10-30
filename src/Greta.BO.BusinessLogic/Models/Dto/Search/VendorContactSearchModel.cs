using System.ComponentModel.DataAnnotations;

namespace Greta.BO.BusinessLogic.Models.Dto.Search
{
    public class VendorContactSearchModel : BaseSearchModel
    {
        public string Contact { get; set; }

        [Phone] public string Phone { get; set; }

        [EmailAddress] public string Email { get; set; }
    }
}