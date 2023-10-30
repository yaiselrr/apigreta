using System.ComponentModel.DataAnnotations;

namespace Greta.BO.BusinessLogic.Models.Dto
{
    public class LoginModel
    {
        [Required] public string UserName { get; set; }

        [Required] public string Password { get; set; }

        [Required] public string GrantType { get; set; }

        [Required] public string RefreshToken { get; set; }

        [Required] public string Scope { get; set; }
    }
}