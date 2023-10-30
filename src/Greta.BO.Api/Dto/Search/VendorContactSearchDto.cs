using System.ComponentModel.DataAnnotations;

namespace Greta.BO.Api.Dto.Search
{
    public class VendorContactSearchDto : BaseSearchDto
    {
        public string Contact { get; set; }

        [Phone] public string Phone { get; set; }

        [EmailAddress] public string Email { get; set; }
    }
}