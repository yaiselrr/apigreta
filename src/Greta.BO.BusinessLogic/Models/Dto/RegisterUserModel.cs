using System.ComponentModel.DataAnnotations;

namespace Greta.BO.BusinessLogic.Models.Dto
{
    public class RegisterUserModel
    {
        [Required] public string FistName { get; set; }

        [Required] public string LastName { get; set; }

        [Required] public string UserName { get; set; }

        [Required] [EmailAddress] public string Email { get; set; }

        [Required] public string Password { get; set; }

        [Required] public string ConfirmPassword { get; set; }
    }
}