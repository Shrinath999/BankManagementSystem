using System.ComponentModel.DataAnnotations;

namespace BankingSystem.Web.ViewModels
{
    public class CreateUserViewModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; }
    }
}